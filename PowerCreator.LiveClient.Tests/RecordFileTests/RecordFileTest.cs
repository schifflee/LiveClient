using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.AudioEncoder;
using PowerCreator.LiveClient.Core.Record;
using PowerCreator.LiveClient.Core.VideoDevice;
using PowerCreator.LiveClient.Core.VideoEncoder;
using System.Threading;

namespace PowerCreator.LiveClient.Tests.RecordFileTests
{
    [TestClass]
    public class RecordFileTest : TestBaseWithLocalIocManager
    {
        private readonly IVideoEncoder videoEncoder;
        private readonly IVideoDeviceManager videoDeviceManager;
        private readonly IAacEncoder aacEncoder;
        private readonly IAudioDeviceManager audioDeviceManager;
        public RecordFileTest()
        {
            videoEncoder = Resolve<IVideoEncoder>();
            videoDeviceManager = Resolve<IVideoDeviceManager>();
            aacEncoder = Resolve<IAacEncoder>();
            audioDeviceManager = Resolve<IAudioDeviceManager>();
        }
        [TestMethod]
        public void RecordFileTestMethod()
        {
            var videoDevice = videoDeviceManager.GetVideoDeviceById(0);
            videoEncoder.SetVideoSource(videoDevice);

            var audioDevice = audioDeviceManager.GetAudioDeviceById(0);
            aacEncoder.SetAudioDataSource(audioDevice);

            Record record = new Record(videoEncoder, aacEncoder);
            Assert.IsTrue(record.StartRecord(@"test.mp4"));
            Thread.Sleep(3000);
            Assert.IsTrue(record.StopRecord());
        }
        [TestMethod]
        public void VideoSourceSwitchTest()
        {
            var videoDevice = videoDeviceManager.GetVideoDeviceById(0);
            videoEncoder.SetVideoSource(videoDevice);
            var audioDevice = audioDeviceManager.GetAudioDeviceById(0);
            aacEncoder.SetAudioDataSource(audioDevice);

            Record record = new Record(videoEncoder, aacEncoder);
            Assert.IsTrue(record.StartRecord(@"test.mp4"));
            Thread.Sleep(3000);
            Assert.IsTrue(videoEncoder.SetVideoSource(videoDeviceManager.GetVideoDeviceById(1)));
            Thread.Sleep(3000);
            Assert.IsTrue(record.StopRecord());
        }
    }
}
