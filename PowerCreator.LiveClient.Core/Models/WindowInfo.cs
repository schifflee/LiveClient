using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.Models
{
    public class WindowInfo
    {
        public IntPtr HWD { get; set; }
        public string WindowTitle { get; set; }
        public override string ToString()
        {
            return WindowTitle;
        }
    }
}
