namespace Uncas.Core.Web.WebControls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Media player that supports the following formats:
    ///     Movie formats: wmv, flv 
    ///     Sound formats: wma, mp3
    /// </summary>
    [ToolboxData("<{0}:MediaPlayer runat=server></{0}:MediaPlayer>")]
    public class MediaPlayer : BasePlayer
    {
        #region Player formats

        private string GetFlashPlayerFormat()
        {
            string FlashPlayerLocation = Page.ClientScript.GetWebResourceUrl
                (this.GetType(), "Uncas.Core.Web.WebControls.FlashPlayer.swf");
            return @"
<div id='{0}_flashContainer'>
    <a href='http://www.macromedia.com/go/getflashplayer'>Get the Flash Player</a> to see this player.
</div>
<script type='text/javascript'>
	var s1 = new SWFObject('" + FlashPlayerLocation + @"', 'mediaplayer', '{2}', '{3}', '8');
	s1.addParam('allowfullscreen', 'true');
	s1.addVariable('width', '{2}');
	s1.addVariable('height', '{3}');
	s1.addVariable('file', '{1}');
	s1.write('{0}_flashContainer');
</script>
";
        }

        private static string GetMp4PlayerFormat()
        {
            return @"
<object classid='clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B' 
    codebase='http://www.apple.com/qtactivex/qtplugin.cab' id='{0}_object'
    width='{2}' height='{3}'>
    <param name='src' value='{1}' />
    <param name='autoplay' value='{4}' />
    <embed src='{1}' type='image/x-macpaint' id='{0}_embed' 
        pluginspage='http://www.apple.com/quicktime/download' width='{2}' height='{3}' 
        autoplay='{4}'>
    </embed>
</object>";
        }

        private static string GetIEMediaPlayerFormat()
        {
            return @"
<object classid='CLSID:22D6F312-B0F6-11D0-94AB-0080C74C7E95'
    codebase='http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701'
    standby='Loading Microsoft® Windows® Media Player components...'
    type='application/x-oleobject'
    id='{0}_object'
    width='{2}px'
    height='{3}px'
    align='middle'>
    <param name='AnimationAtStart' value='True' />
    <param name='AutoStart' value='{4}' />
    <param name='DefaultFrame' value='mainFrame' />
    <param name='FileName' value='{1}' />
    <param name='Loop' value='True' />
    <param name='ShowControls' value='True' />
    <param name='ShowDisplay' value='False' />
    <param name='ShowStatusBar' value='False' />
    <param name='TransparentAtStart' value='0' />
</object>
";
        }

        private static string GetNonIEMediaPlayerFormat()
        {
            return @"
    <embed 
        pluginspage='http://www.microsoft.com/Windows/MediaPlayer/'
        type='application/x-mplayer2' 
        designtimesp='5311' 
        id='{0}_embed' 
        name='{0}_embed' 
        width='{2}px' 
        height='{3}px'
        align='middle' 
        autosize='False' 
        autostart='{4}'
        bgcolor='darkblue' 
        defaultframe='rightFrame' 
        displaysize='4'
        loop='0'
        showcontrols='1' 
        showdisplay='0'
        showstatusbar='false' 
        showtracker='0' 
        src='{1}'
        videoborder3d='0' 
        >
    </embed>
";
        }

        #endregion

        #region PreRender and Render

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.MediaSourceHasExtension(".flv"))
            {
                string swfobjectLocation =
                    Page.ClientScript.GetWebResourceUrl(this.GetType(), "Uncas.Core.Web.WebControls.swfobject.js");
                Page.ClientScript.RegisterClientScriptInclude("swfobject", swfobjectLocation);
            }
        }

        private bool MediaSourceHasExtension(string extension)
        {
            return this.MediaSource.EndsWith(
                extension,
                StringComparison.OrdinalIgnoreCase);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            // Resizing when playing sound:
            if (this.MediaSourceHasExtension(".mp3") ||
                this.MediaSourceHasExtension(".wma"))
            {
                if (IsIE())
                {
                    this.Width = Unit.Pixel(300);
                    this.Height = Unit.Pixel(42);
                }
                else
                {
                    this.Width = Unit.Pixel(210);
                    this.Height = Unit.Pixel(45);
                }
            }

            // Getting the media player html:
            string mediaPlayerFormat = string.Empty;
            if (this.MediaSourceHasExtension(".flv"))
            {
                mediaPlayerFormat = GetFlashPlayerFormat();
            }
            else if (this.MediaSourceHasExtension(".mp4")
                || this.MediaSourceHasExtension(".m4v")
                || this.MediaSourceHasExtension(".mov"))
            {
                mediaPlayerFormat = GetMp4PlayerFormat();
            }
            else if (IsIE())
            {
                mediaPlayerFormat = GetIEMediaPlayerFormat();
            }
            else
            {
                mediaPlayerFormat = GetNonIEMediaPlayerFormat();
            }

            string mediaPlayer =
                string.Format(
                mediaPlayerFormat
                /* 0 */, this.ClientID
                /* 1 */, this.MediaSource
                /* 2 */, (int)this.Width.Value
                /* 3 */, (int)this.Height.Value
                /* 4 */, this.AutoPlay
                );
            writer.Write(mediaPlayer);
        }

        #endregion

        private bool IsIE()
        {
            return Page.Request.Browser.Browser.Equals(
                "ie",
                StringComparison.OrdinalIgnoreCase);
        }
    }
}