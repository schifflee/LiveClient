using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerCreator.LiveClient.Core.VideoDevice;
using System.Linq;

namespace PowerCreator.LiveClient.Tests.VideoDeviceTests
{
    [TestClass]
    public class VideoDeviceManagerTest : TestBaseWithLocalIocManager
    {
        
        [TestMethod]
        public void ShouldGetAllLocalVideoDevices()
        {
            IVideoDeviceManager videoDeviceManager = Resolve<IVideoDeviceManager>();

            var deviceList = videoDeviceManager.GetVideoDevices();

            Assert.IsNotNull(deviceList);

            Assert.IsTrue(deviceList.Any());

            videoDeviceManager.Dispose();
        }
        [TestMethod]
        public void ShouldGetLocalVideoDeviceByDeviceId()
        {
            IVideoDeviceManager videoDeviceManager = Resolve<IVideoDeviceManager>();

            IVideoDevice videoDevice = videoDeviceManager.GetVideoDeviceById(0);

            Assert.IsNotNull(videoDevice);

            Assert.IsTrue(videoDevice.ID == 0);

            videoDeviceManager.Dispose();
        }
    }
}
