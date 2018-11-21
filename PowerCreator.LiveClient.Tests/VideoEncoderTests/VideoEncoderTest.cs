using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Core.VideoDevice;
using PowerCreator.LiveClient.Core.VideoEncoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Tests.VideoEncoderTests
{
    [TestClass]
    public class VideoEncoderTest : TestBaseWithLocalIocManager, IObserver<VideoEncodedDataContext>
    {
        protected IDisposable unsubscriber;

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(VideoEncodedDataContext value)
        {
            Debug.WriteLine("VideoEncoderTest:" + value.DataLength);
        }

        [TestMethod]
        public void SourceVideoDataReceiveAndEncodedDataSend()
        {
            var videoEncoder = Resolve<IVideoEncoder>();
            var videoDeviceManager = Resolve<IVideoDeviceManager>();
            var videoDevice = videoDeviceManager.GetVideoDeviceById(0);
            Assert.IsTrue(videoEncoder.SetVideoSource(videoDevice));
            videoEncoder.StartVideoEncoder();
            unsubscriber = videoEncoder.Subscribe(this);
            Assert.IsTrue(videoEncoder.IsStartEncoder);
            Assert.IsTrue(videoDevice.IsOpen);
            Thread.Sleep(5000);
            unsubscriber.Dispose();
            Thread.Sleep(5000);
            videoEncoder.StopVideoEncoder();
            Assert.IsFalse(videoDevice.IsOpen);
            Assert.IsFalse(videoEncoder.IsStartEncoder);
            videoEncoder.Dispose();
            videoDeviceManager.Dispose();

        }
    }
}
