using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerCreator.LiveClient.Core.AudioDevice;

namespace PowerCreator.LiveClient.Tests.AudioDeviceTests
{
    [TestClass]
    public class AudioDeviceTest : TestBaseWithLocalIocManager
    {
        [TestMethod]
        public void OpenAndCloseAudioDeviceTest()
        {
            var adm = Resolve<IAudioDeviceManager>();
            foreach (var device in adm.GetAudioDevices())
            {
                device.OpenDevice();
                Assert.IsTrue(device.IsOpen);
                device.CloseDevice();
                Assert.IsFalse(device.IsOpen);
            }
            adm.Dispose();
        }
    }
}
