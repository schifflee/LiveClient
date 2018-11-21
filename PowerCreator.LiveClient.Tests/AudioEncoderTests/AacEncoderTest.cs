using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.AudioEncoder;
using PowerCreator.LiveClient.Core.Models;
using System;
using System.Diagnostics;
using System.Threading;

namespace PowerCreator.LiveClient.Tests.AudioEncoderTests
{
    [TestClass]
    public class AacEncoderTest : TestBaseWithLocalIocManager, IObserver<AudioEncodedDataContext>
    {
        protected IDisposable unsubscriber;
        [TestMethod]
        public void SourceAudioDataReceiveAndEncodedDataSend()
        {
            var aacEncoder = Resolve<IAacEncoder>();
            var audioDeviceManager = Resolve<IAudioDeviceManager>();
            var audioDevice = audioDeviceManager.GetAudioDeviceById(0);
            Assert.IsTrue(aacEncoder.SetAudioDataSource(audioDevice));
            aacEncoder.StartAudioEncoder();
            unsubscriber = aacEncoder.Subscribe(this);
            Assert.IsTrue(aacEncoder.IsStartEncoder);
            Assert.IsTrue(audioDevice.IsOpen);
            Thread.Sleep(5000);
            aacEncoder.StopAudioEncoder();
            Assert.IsFalse(audioDevice.IsOpen);
            Assert.IsFalse(aacEncoder.IsStartEncoder);
            unsubscriber.Dispose();
            aacEncoder.Dispose();
            audioDeviceManager.Dispose();

        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(AudioEncodedDataContext value)
        {
            Debug.WriteLine("AacEncoderTest:" + value.DataLength);
        }
    }
}
