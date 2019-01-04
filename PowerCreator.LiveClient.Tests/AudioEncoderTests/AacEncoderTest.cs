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
    public class AacEncoderTest : TestBaseWithLocalIocManager
    {
        [TestMethod]
        public void SourceAudioDataReceiveAndEncodedDataSend()
        {
            var aacEncoder = Resolve<IAacEncoder>();
            var audioDeviceManager = Resolve<IAudioDeviceManager>();
            var audioDevice = audioDeviceManager.GetAudioDeviceById(0);
            Assert.IsTrue(aacEncoder.SetAudioDataSource(audioDevice));
            aacEncoder.StartAudioEncoder();
            aacEncoder.PushingData += AacEncoder_PushingData;
            Assert.IsTrue(aacEncoder.IsStartEncoder);
            Assert.IsTrue(audioDevice.IsOpen);
            Thread.Sleep(5000);
            aacEncoder.StopAudioEncoder();
            Assert.IsFalse(audioDevice.IsOpen);
            Assert.IsFalse(aacEncoder.IsStartEncoder);
            aacEncoder.PushingData -= AacEncoder_PushingData;
            aacEncoder.Dispose();
            audioDeviceManager.Dispose();

        }

        private void AacEncoder_PushingData(AudioEncodedDataContext value)
        {
            Debug.WriteLine("AacEncoderTest:" + value.DataLength);
        }
    }
}
