namespace Uncas.Core.Web.WebControls
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [DefaultProperty("MediaSource")]
    public abstract class BasePlayer : WebControl
    {
        private bool _autoPlay;

        private Unit _width = new Unit(480);
        
        private Unit _Height = new Unit(360);

        public bool AutoPlay
        {
            get { return _autoPlay; }
            set { _autoPlay = value; }
        }

        public override Unit Height
        {
            get
            {
                base.Height = _Height;
                return _Height;
            }
            set
            {
                base.Height = value;
                _Height = value;
            }
        }

        [Bindable(true)]
        [DefaultValue("")]
        [UrlProperty]
        public string MediaSource
        {
            get
            {
                string s = (string)ViewState["MediaSource"];
                return string.IsNullOrEmpty(s) ? string.Empty : s;
            }

            set
            {
                string mediaSource = value;
                if (!mediaSource.StartsWith(
                    "http",
                    StringComparison.OrdinalIgnoreCase))
                {
                    mediaSource = string.Format("{0}/{1}"
                        , Uncas.Core.Web.SiteUrl.BaseUrl
                        , mediaSource.TrimStart('/', '~'));
                }

                ViewState["MediaSource"] = mediaSource;
            }
        }

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