namespace Uncas.Core.Web.WebControls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Base player for all media players.
    /// </summary>
    [DefaultProperty("MediaSource")]
    public abstract class BasePlayer : WebControl
    {
        private Unit _height = new Unit(360);
        private Unit _width = new Unit(480);

        /// <summary>
        /// Gets or sets a value indicating whether to auto play.
        /// </summary>
        /// <value>
        ///   <c>True</c> if set to auto play; otherwise, <c>false</c>.
        /// </value>
        public bool AutoPlay { get; set; }

        /// <summary>
        /// Gets or sets the height of the Web server control.
        /// </summary>
        /// <value>The height.</value>
        /// <returns>A <see cref="T:System.Web.UI.WebControls.Unit"/> that represents the height of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"/>.</returns>
        /// <exception cref="T:System.ArgumentException">The height was set to a negative value.</exception>
        public override Unit Height
        {
            get
            {
                base.Height = _height;
                return _height;
            }

            set
            {
                base.Height = value;
                _height = value;
            }
        }

        /// <summary>
        /// Gets or sets the media source.
        /// </summary>
        /// <value>
        /// The media source.
        /// </value>
        [Bindable(true)]
        [DefaultValue("")]
        [UrlProperty]
        public string MediaSource
        {
            get
            {
                var s = (string)ViewState["MediaSource"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                string mediaSource = value;
                if (!mediaSource.StartsWith(
                    "http",
                    StringComparison.OrdinalIgnoreCase))
                {
                    mediaSource = string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}/{1}",
                        SiteUrl.BaseUrl,
                        mediaSource.TrimStart('/', '~'));
                }

                ViewState["MediaSource"] = mediaSource;
            }
        }

        /// <summary>
        /// Gets or sets the width of the Web server control.
        /// </summary>
        /// <value>The width.</value>
        /// <returns>A <see cref="T:System.Web.UI.WebControls.Unit"/> that represents the width of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"/>.</returns>
        /// <exception cref="T:System.ArgumentException">The width of the Web server control was set to a negative value. </exception>
        public override Unit Width
        {
            get
            {
                base.Width = _width;
                return _width;
            }

            set
            {
                base.Width = value;
                _width = value;
            }
        }
    }
}