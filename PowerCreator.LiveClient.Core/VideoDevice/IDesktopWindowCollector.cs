using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.VideoDevice
{
    public interface IDesktopWindowCollector : IVideoDevice
    {
        bool SetWindowHandle(IntPtr intPtr);
    }
}
