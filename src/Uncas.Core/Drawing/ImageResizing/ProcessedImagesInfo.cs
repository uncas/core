namespace Uncas.Core.Drawing.ImageResizing
{
    using System.Globalization;

    /// <summary>
    /// Contains info about processed images.
    /// </summary>
    public class ProcessedImagesInfo
    {
        /// <summary>
        /// Gets or sets the total number of images.
        /// </summary>
        /// <value>The total number of images.</value>
        public int TotalNumberOfImages { get; set; }

        /// <summary>
        /// Gets or sets the resized number of images.
        /// </summary>
        /// <value>The resized number of images.</value>
        public int ResizedNumberOfImages { get; set; }

        /// <summary>
        /// Gets the percentage.
        /// </summary>
        /// <value>The percentage.</value>
        public int Percentage
        {
            get
            {
                return (int)((100d * ResizedNumberOfImages)
                             / (1d * TotalNumberOfImages));
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}/{1}",
                ResizedNumberOfImages,
                TotalNumberOfImages);
        }
    }
}