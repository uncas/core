namespace Uncas.Core.Web.WebControls
{
    using System.Web.UI;

    /// <summary>
    /// Playback of videos on YouTube, VideoJug, FiveMinutes, EHow
    /// </summary>
    [ToolboxData("<{0}:ExternalPlayer runat=server></{0}:ExternalPlayer>")]
    public class ExternalPlayer : BasePlayer
    {
        private VideoSourceType _mediaSourceType = VideoSourceType.YouTube;

        /// <summary>
        /// Gets or sets the type of the media source.
        /// </summary>
        /// <value>
        /// The type of the media source.
        /// </value>
        public VideoSourceType MediaSourceType
        {
            get
            {
                return _mediaSourceType;
            }
            set
            {
                _mediaSourceType = value;
            }
        }

        #region Formats

        private static string GetYouTubeFormat()
        {
            return @"
<object id='{0}_object' width='{1}' height='{2}'>
    <param name='movie' value='http://www.youtube.com/v/{3}&hl=en&fs=1' />
    <param name='allowFullScreen' value='true' />
    <embed id='{0}_embed' src='http://www.youtube.com/v/{3}&hl=en&fs=1' 
        type='application/x-shockwave-flash' 
        allowfullscreen='true' 
        width='{1}' 
        height='{2}'>
    </embed>
</object>
";
        }

        private static string GetVideoJugFormat()
        {
            return @"
<object 
    classid='clsid:d27cdb6e-ae6d-11cf-96b8-444553540000' 
    codebase='http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0'
    id='{0}_object' 
    width='{1}' 
    height='{2}' 
    align='middle' 
    allowfullscreen='true'>
    <param name='movie' value='http://www.videojug.com/film/player?id={3}' />
    <param value='true' name='allowFullScreen' />
    <param value='always' name='allowScriptAccess' />
    <embed id='{0}_embed'
        src='http://www.videojug.com/film/player?id={3}'
        quality='high' 
        width='{1}' 
        height='{2}' 
        type='application/x-shockwave-flash'
        pluginspage='http://www.macromedia.com/go/getflashplayer' 
        allowscriptaccess='always'
        allowfullscreen='true'>
    </embed>
</object>
";
        }

        private static string Get5minFormat()
        {
            return @"
<object width='{1}' height='{2}' id='{0}_object'>
    <param name='allowfullscreen' value='true' />
    <param name='allowScriptAccess' value='always' />
    <param name='movie' value='http://www.5min.com/Embeded/{3}/' />
    <embed id='{0}_embed' src='http://www.5min.com/Embeded/{3}/' type='application/x-shockwave-flash'
        width='{1}' height='{2}' allowfullscreen='true' allowscriptaccess='always'>
    </embed>
</object>
";
        }

        private static string GetEHowFormat()
        {
            return @"
<embed id='{0}_embed' 
    name='{0}_embed'
    width='{1}' 
    height='{2}' 
    align='TL' 
    flashvars='id={3}&partnerId=3&pwidth={1}&pheight={2}'
    scale='noscale' 
    allowfullscreen='true' 
    wmode='window' 
    menu='false' 
    loop='false'
    allowscriptaccess='always' 
    quality='high' 
    bgcolor='#000000' 
    style='' 
    src='http://www.ehow.com/flash/player.swf'
    type='application/x-shockwave-flash' />
";
        }

        #endregion

        #region Render

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            string playerFormat = string.Empty;
            switch (this.MediaSourceType)
            {
                case VideoSourceType.YouTube:
                    playerFormat = GetYouTubeFormat();
                    break;
                case VideoSourceType.VideoJug:
                    playerFormat = GetVideoJugFormat();
                    break;
                case VideoSourceType.FiveMinutes:
                    playerFormat = Get5minFormat();
                    break;
                case VideoSourceType.EHow:
                    playerFormat = GetEHowFormat();
                    break;
                default:
                    break;
            }

            string player = string.Format(playerFormat
                , this.ClientID
                , (int)this.Width.Value
                , (int)this.Height.Value
                , this.MediaSource
                );

            writer.Write(player);
        }

        #endregion
    }
}