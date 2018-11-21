using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerCreator.LiveClient.Core.AudioDevice;
using System.Linq;

namespace PowerCreator.LiveClient.Tests.AudioDeviceTests
{
    [TestClass]
    public class AudioDeviceManagerTest:TestBaseWithLocalIocManager
    {
        [TestMethod]
        public void ShouldGetAllLocalAudioDevices()
        {
            var audioDeviceManager = Resolve<IAudioDeviceManager>();

            var deviceList = audioDeviceManager.GetAudioDevices();

            Assert.IsNotNull(deviceList);

            Assert.IsTrue(deviceList.Any());

            audioDeviceManager.Dispose();
        }
        [TestMethod]
        public void ShouldGetLocalAudioDeviceByDeviceId()
        {
            var audioDeviceManager = Resolve<IAudioDeviceManager>();

            var  videoDevice = audioDeviceManager.GetAudioDeviceById(0);

            Assert.IsNotNull(videoDevice);

            Assert.IsTrue(videoDevice.ID == 0);
            audioDeviceManager.Dispose();
        }
    }
}
