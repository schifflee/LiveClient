using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerCreator.LiveClient.Core.VideoDevice;
using Microsoft.Practices.Unity;

namespace PowerCreator.LiveClient.Tests.VideoDeviceTests
{
    [TestClass]
    public class VideoDeviceTest : TestBaseWithLocalIocManager
    {
        [TestMethod]
        public void OpenAndCloseVideoDeviceTest()
        {
            var vdm = Resolve<IVideoDeviceManager>();
            foreach (var device in vdm.GetVideoDevices())
            {
                device.OpenDevice();
                Assert.IsTrue(device.IsAvailable ? device.IsOpen : true);
                device.CloseDevice();
                Assert.IsFalse(device.IsOpen);
            }
            vdm.Dispose();
        }

    }
}
