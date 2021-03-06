﻿namespace Uncas.Core.Web.WebControls
{
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Media player that supports the following formats:
    ///     Movie formats: wmv, flv 
    ///     Sound formats: wma, mp3.
    /// </summary>
    [ToolboxData("<{0}:MediaPlayer runat=server></{0}:MediaPlayer>")]
    public class MediaPlayer : BasePlayer
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (MediaSourceHasExtension(".flv"))
            {
                string swfobjectLocation =
                    Page.ClientScript.GetWebResourceUrl(GetType(), "Uncas.Core.Web.WebControls.swfobject.js");
                Page.ClientScript.RegisterClientScriptInclude("swfobject", swfobjectLocation);
            }
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (writer == null)
            {
                return;
            }

            // Resizing when playing sound:
            if (MediaSourceHasExtension(".mp3") ||
                MediaSourceHasExtension(".wma"))
            {
                if (IsIE())
                {
                    Width = Unit.Pixel(300);
                    Height = Unit.Pixel(42);
                }
                else
                {
                    Width = Unit.Pixel(210);
                    Height = Unit.Pixel(45);
                }
            }

            // Getting the media player html:
            string mediaPlayerFormat = string.Empty;
            if (MediaSourceHasExtension(".flv"))
            {
                mediaPlayerFormat = GetFlashPlayerFormat();
            }
            else if (MediaSourceHasExtension(".mp4")
                     || MediaSourceHasExtension(".m4v")
                     || MediaSourceHasExtension(".mov"))
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
                    CultureInfo.InvariantCulture,
                    mediaPlayerFormat,
                    /* 0 */ ClientID,
                    /* 1 */ MediaSource,
                    /* 2 */ (int)Width.Value,
                    /* 3 */ (int)Height.Value,
                    /* 4 */ AutoPlay);
            writer.Write(mediaPlayer);
        }

        private static string GetIEMediaPlayerFormat()
        {
            return
                @"
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

        private static string GetMp4PlayerFormat()
        {
            return
                @"
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

        private static string GetNonIEMediaPlayerFormat()
        {
            return
                @"
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

        private string GetFlashPlayerFormat()
        {
            string flashPlayerLocation = Page.ClientScript.GetWebResourceUrl(
                GetType(),
                "Uncas.Core.Web.WebControls.FlashPlayer.swf");
            return
                @"
<div id='{0}_flashContainer'>
    <a href='http://www.macromedia.com/go/getflashplayer'>Get the Flash Player</a> to see this player.
</div>
<script type='text/javascript'>
	var s1 = new SWFObject('" +
                flashPlayerLocation +
                @"', 'mediaplayer', '{2}', '{3}', '8');
	s1.addParam('allowfullscreen', 'true');
	s1.addVariable('width', '{2}');
	s1.addVariable('height', '{3}');
	s1.addVariable('file', '{1}');
	s1.write('{0}_flashContainer');
</script>
";
        }

        private bool IsIE()
        {
            return Page.Request.Browser.Browser.Equals(
                "ie",
                StringComparison.OrdinalIgnoreCase);
        }

        private bool MediaSourceHasExtension(string extension)
        {
            return MediaSource.EndsWith(
                extension,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}