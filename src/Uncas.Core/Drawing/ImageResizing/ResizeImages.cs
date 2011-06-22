using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Uncas.Core.Drawing.ImageResizing
{
    /// <summary>
    /// Resizes images.
    /// </summary>
    public class ResizeImages : IDisposable, IResizeImages
    {
        #region Private fields

        private BackgroundWorker resizeWorker;

        private ImageHandler ih;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeImages"/> class.
        /// </summary>
        public ResizeImages()
        {
            this.resizeWorker = new BackgroundWorker();
            this.resizeWorker.WorkerReportsProgress = true;
            this.resizeWorker.WorkerSupportsCancellation = true;

            this.resizeWorker.DoWork
                += new DoWorkEventHandler(resizeWorker_DoWork);
            this.resizeWorker.ProgressChanged
                += new ProgressChangedEventHandler(resizeWorker_ProgressChanged);
            this.resizeWorker.RunWorkerCompleted
                += new RunWorkerCompletedEventHandler(resizeWorker_RunWorkerCompleted);

            this.ih = new ImageHandler();
        }

        #endregion

        #region Public events

        /// <summary>
        /// Occurs when resize progress changed.
        /// </summary>
        public event EventHandler<ResizeProgressEventArgs>
            ResizeProgressChanged;

        /// <summary>
        /// Occurs when resize completed.
        /// </summary>
        public event EventHandler<ResizeCompletedEventArgs>
            ResizeCompleted;

        /// <summary>
        /// Occurs when resize failed.
        /// </summary>
        public event EventHandler<ResizeFailedEventArgs>
            ResizeFailed;

        #endregion

        #region Public methods

        /// <summary>
        /// Does the resize work async.
        /// </summary>
        /// <param name="baseOutputFolder">The base output folder.</param>
        /// <param name="chooseFiles">if set to <c>true</c> [choose files].</param>
        /// <param name="chooseFolder">if set to <c>true</c> [choose folder].</param>
        /// <param name="baseInputFolder">The base input folder.</param>
        /// <param name="includeSubFolders">if set to <c>true</c> [include sub folders].</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="maxImageSize">Maximum size of images.</param>
        public void DoResizeWorkAsync(
            string baseOutputFolder,
            int maxImageSize,
            bool chooseFiles,
            IEnumerable filePaths,
            bool chooseFolder,
            string baseInputFolder,
            bool includeSubFolders)
        {
            // Gets the list of images to resize:
            List<ImageToResize> imagesToResize =
                GetListOfImages(
                baseOutputFolder,
                chooseFiles,
                filePaths,
                chooseFolder,
                baseInputFolder,
                includeSubFolders);

            // Resizes the images:
            SelectedImagesInfo sfi = new SelectedImagesInfo
            {
                ImagesToResize = imagesToResize,
                MaxImageSize = maxImageSize
            };
            resizeWorker.RunWorkerAsync(sfi);
        }

        private List<ImageToResize> GetListOfImages(
            string baseOutputFolder,
            bool chooseFiles,
            IEnumerable filePaths,
            bool chooseFolder,
            string baseInputFolder,
            bool includeSubFolders)
        {
            var imagesToResize = new List<ImageToResize>();
            if (chooseFiles)
            {
                imagesToResize
                    = GetSelectedImages(
                    baseOutputFolder,
                    filePaths);
            }
            else if (chooseFolder)
            {
                DirectoryInfo diBase
                    = new DirectoryInfo(baseInputFolder);
                try
                {
                    GetImagesInSelectedFolder(
                        ref imagesToResize,
                        diBase.Name,
                        baseOutputFolder,
                        diBase,
                        includeSubFolders);
                }
                catch (IOException ex)
                {
                    HandleException(ex);
                }
            }

            return imagesToResize;
        }

        /// <summary>
        /// Cancels the resize work.
        /// </summary>
        public void CancelResizeWork()
        {
            resizeWorker.CancelAsync();
        }

        #endregion

        #region Private methods

        #region Running in background thread

        private void resizeWorker_DoWork(
            object sender,
            DoWorkEventArgs e)
        {
            SelectedImagesInfo sfi = (SelectedImagesInfo)e.Argument;
            int filesCompleted = 0;
            ProcessedImagesInfo pif = new ProcessedImagesInfo
            {
                TotalNumberOfImages = sfi.ImagesToResize.Count,
                ResizedNumberOfImages = filesCompleted
            };
            foreach (ImageToResize itr in sfi.ImagesToResize)
            {
                pif.ResizedNumberOfImages = filesCompleted;
                resizeWorker.ReportProgress(pif.Percentage, pif);
                if (resizeWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                ResizeImage(
                    itr.OriginalImagePath,
                    itr.ResizedImagePath,
                    sfi.MaxImageSize);
                filesCompleted++;
            }

            pif.ResizedNumberOfImages = filesCompleted;
            e.Result = pif;
        }

        private void resizeWorker_ProgressChanged(
            object sender,
            ProgressChangedEventArgs e)
        {
            ProcessedImagesInfo pif
                = (ProcessedImagesInfo)e.UserState;
            if (ResizeProgressChanged != null)
            {
                ResizeProgressChanged(
                    this,
                    new ResizeProgressEventArgs(pif));
            }
        }

        private void resizeWorker_RunWorkerCompleted(
            object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (this.ResizeCompleted != null)
            {
                this.ResizeCompleted(
                    this,
                    new ResizeCompletedEventArgs(
                        e.Cancelled,
                        (ProcessedImagesInfo)e.Result));
            }
        }

        #endregion

        #region The actual resizing methods

        private static List<ImageToResize> GetSelectedImages(
            string baseOutputFolder,
            IEnumerable filePaths)
        {
            List<ImageToResize> imagesToResize
                = new List<ImageToResize>();
            foreach (string filePath in filePaths)
            {
                FileInfo fi = new FileInfo(filePath);
                string smallerFilePath =
                    Path.Combine(baseOutputFolder, fi.Name);
                imagesToResize.Add(new ImageToResize
                {
                    OriginalImagePath = filePath,
                    ResizedImagePath = smallerFilePath
                });
            }

            return imagesToResize;
        }

        private void GetImagesInSelectedFolder(
            ref List<ImageToResize> imagesToResize,
            string relativeInputFolderPath,
            string baseOutputFolder,
            DirectoryInfo di,
            bool includeSubFolders)
        {
            string outputFolderPath
                = Path.Combine(
                baseOutputFolder,
                relativeInputFolderPath);
            DirectoryInfo diOutput
                = new DirectoryInfo(outputFolderPath);

            // Getting images in this folder
            GetFilesByExtension(
                imagesToResize,
                di,
                outputFolderPath,
                diOutput,
                "*.jpg");
            GetFilesByExtension(
                imagesToResize,
                di,
                outputFolderPath,
                diOutput,
                "*.jpeg");
            GetFilesByExtension(
                imagesToResize,
                di,
                outputFolderPath,
                diOutput,
                "*.bmp");
            GetFilesByExtension(
                imagesToResize,
                di,
                outputFolderPath,
                diOutput,
                "*.png");
            GetFilesByExtension(
                imagesToResize,
                di,
                outputFolderPath,
                diOutput,
                "*.gif");
            if (includeSubFolders)
            {
                // Resizing images in subfolders
                foreach (DirectoryInfo diChild in di.GetDirectories())
                {
                    string childRelativePath
                        = Path.Combine(
                        relativeInputFolderPath,
                        diChild.Name);
                    GetImagesInSelectedFolder(
                        ref imagesToResize,
                        childRelativePath,
                        baseOutputFolder,
                        diChild,
                        includeSubFolders);
                }
            }
        }

        private static void GetFilesByExtension(
            List<ImageToResize> imagesToResize,
            DirectoryInfo di,
            string outputFolderPath,
            DirectoryInfo diOutput,
            string extension)
        {
            foreach (FileInfo fi in di.GetFiles(extension))
            {
                if (!diOutput.Exists)
                {
                    diOutput.Create();
                }

                string originalImagePath = fi.FullName;
                string resizedImagePath
                    = Path.Combine(outputFolderPath, fi.Name);
                imagesToResize.Add(new ImageToResize
                {
                    OriginalImagePath = originalImagePath,
                    ResizedImagePath = resizedImagePath
                });
            }
        }

        private void ResizeImage(
            string originalImagePath,
            string resizedImagePath,
            int maxImageSize)
        {
            try
            {
                if (File.Exists(originalImagePath)
                    && !File.Exists(resizedImagePath))
                {
                    byte[] originalBytes
                        = File.ReadAllBytes(originalImagePath);
                    byte[] resizedBytes
                        = ih.GetThumbnail(originalBytes, maxImageSize);
                    originalBytes = null;
                    File.WriteAllBytes(resizedImagePath, resizedBytes);
                    resizedBytes = null;
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (IOException ex)
            {
                HandleException(ex);
            }
        }

        #endregion

        private void HandleException(Exception ex)
        {
            if (this.ResizeFailed != null)
            {
                this.ResizeFailed(
                    this,
                    new ResizeFailedEventArgs(ex));
            }
        }

        #endregion

        #region Private classes

        private class ImageToResize
        {
            public string OriginalImagePath { get; set; }

            public string ResizedImagePath { get; set; }
        }

        private class SelectedImagesInfo
        {
            public int MaxImageSize { get; set; }

            public List<ImageToResize> ImagesToResize { get; set; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.resizeWorker.Dispose();
            }
        }

        #endregion
    }
}