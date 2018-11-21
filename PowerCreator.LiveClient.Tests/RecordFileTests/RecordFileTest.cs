using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.AudioEncoder;
using PowerCreator.LiveClient.Core.Record;
using PowerCreator.LiveClient.Core.VideoDevice;
using PowerCreator.LiveClient.Core.VideoEncoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Tests.RecordFileTests
{
    [TestClass]
    public class RecordFileTest : TestBaseWithLocalIocManager
    {
        [TestMethod]
        public void RecordFileTestMethod()
        {
            var videoEncoder = Resolve<IVideoEncoder>();
            var videoDeviceManager = Resolve<IVideoDeviceManager>();
            var videoDevice = videoDeviceManager.GetVideoDeviceById(0);

            videoEncoder.SetVideoSource(videoDevice);

            var aacEncoder = Resolve<IAacEncoder>();
            var audioDeviceManager = Resolve<IAudioDeviceManager>();
            var audioDevice = audioDeviceManager.GetAudioDeviceById(0);

            aacEncoder.SetAudioDataSource(audioDevice);

            Record record = new Record(videoEncoder, aacEncoder);
            record.StartRecord(@"E:\项目\LiveClient\1.mp4");
            Thread.Sleep(10000);
            record.StopRecord();
        }
    }
}
