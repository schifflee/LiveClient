using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.VideoDevice
{
    public interface IVideoDeviceManager : IDisposable
    {
        IEnumerable<IVideoDevice> GetVideoDevices();

        IVideoDevice GetVideoDeviceById(int id);
    }
}
