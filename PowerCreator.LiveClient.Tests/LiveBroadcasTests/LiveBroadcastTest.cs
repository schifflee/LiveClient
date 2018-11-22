using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.AudioEncoder;
using PowerCreator.LiveClient.Core.LiveBroadcast;
using PowerCreator.LiveClient.Core.VideoDevice;
using PowerCreator.LiveClient.Core.VideoEncoder;
using System.Threading;

namespace PowerCreator.LiveClient.Tests.LiveBroadcasTests
{
    [TestClass]
    public class LiveBroadcastTest : TestBaseWithLocalIocManager
    {
        [TestMethod]
        public void LivePushStreamTest()
        {
            var videoEncoder = Resolve<IVideoEncoder>();
            var videoDeviceManager = Resolve<IVideoDeviceManager>();
            var videoDevice = videoDeviceManager.GetVideoDeviceById(0);

            videoEncoder.SetVideoSource(videoDevice);

            var aacEncoder = Resolve<IAacEncoder>();
            var audioDeviceManager = Resolve<IAudioDeviceManager>();
            var audioDevice = audioDeviceManager.GetAudioDeviceById(1);

            aacEncoder.SetAudioDataSource(audioDevice);

            LiveBroadcast liveBroadcast = new LiveBroadcast(videoEncoder, aacEncoder);

            Assert.IsTrue(liveBroadcast.StartLive("192.168.0.202", 1935, "live", "test"));
            Thread.Sleep(3000);
            Assert.IsTrue(liveBroadcast.StopLive());
        }
    }
}
