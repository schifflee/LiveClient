using System;

namespace LiveClientDesktop.Models
{
    public class PreviewWindowInfo
    {
        public IntPtr HWD
        {
            get;
            set;
        }

        public string WindowTitle
        {
            get; set;
        }

        public string PreviewWindowImagePath
        {
            get; set;
        }
        public override string ToString()
        {
            return WindowTitle;
        }
    }
}
