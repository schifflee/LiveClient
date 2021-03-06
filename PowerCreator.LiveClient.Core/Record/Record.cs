﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DirectShowLib;
using PowerCreator.LiveClient.Core.AudioEncoder;
using PowerCreator.LiveClient.Core.Enums;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Core.VideoEncoder;
using PowerCreator.LiveClient.VsNetSdk;

namespace PowerCreator.LiveClient.Core.Record
{
    internal class Record : IRecord
    {
        public RecAndLiveState State { get; private set; }

        public bool IsRecord { get; private set; }

        private readonly IntPtr _handle;
        private readonly IVideoEncoder _videoEncoder;
        private readonly IAacEncoder _aacEncoder;
        public Record(IVideoEncoder videoEncoder, IAacEncoder aacEncoder)
        {
            State = RecAndLiveState.NotStart;
            _handle = VsNetRecordSdk.FileMuxer_AllocInstance();
            VsNetRecordSdk.FileMuxer_EnableSync(_handle, true);
            _videoEncoder = videoEncoder;
            _aacEncoder = aacEncoder;
        }


        public bool StartRecord(string recordFileSavePath)
        {
            if (IsRecord) return true;

            _videoEncoder.StartVideoEncoder();
            _aacEncoder.StartAudioEncoder();

            if (SetRecordInfo(recordFileSavePath))
            {
                _videoEncoder.PushingData += VideoEncoderPushingData;
                _aacEncoder.PushingData += AACEncoderPushingData;
                State = RecAndLiveState.Started;
                IsRecord = true;
                return true;
            }
            return false;
        }

        private void AACEncoderPushingData(AudioEncodedDataContext value)
        {
            VsNetRecordSdk.FileMuxer_WriteAudio(_handle, value.Data.ToInt32(), value.DataLength, value.KeyFrame);
        }

        private void VideoEncoderPushingData(VideoEncodedDataContext value)
        {
            VsNetRecordSdk.FileMuxer_WriteVideo(_handle, value.Data, value.DataLength, value.KeyFrame, value.TimeStamp);
        }

        public bool StopRecord()
        {
            if (!IsRecord) return true;

            
            _videoEncoder.PushingData -= VideoEncoderPushingData;
            _aacEncoder.PushingData -= AACEncoderPushingData;
            _videoEncoder.StopVideoEncoder();
            _aacEncoder.StopAudioEncoder();
            if (VsNetRecordSdk.FileMuxer_EndWrite(_handle) == 0)
            {
                State = RecAndLiveState.NotStart;
                IsRecord = false;
                return true;
            }
            return false;
        }

        public bool PauseRecord()
        {
            if (VsNetRecordSdk.FileMuxer_Pause(_handle) == 0)
            {
                State = RecAndLiveState.Pause;
                return true;
            }
            return false;
        }

        public bool ResumeRecord()
        {
            if (VsNetRecordSdk.FileMuxer_Resume(_handle) == 0)
            {
                State = RecAndLiveState.Started;
                return true;
            }
            return false;
        }

        private bool SetRecordInfo(string fileSavePath)
        {
            return VsNetRecordSdk.FileMuxer_BeginWrite(_handle, fileSavePath, _videoEncoder.IntBitmapInfoHeader, Marshal.SizeOf(new BitmapInfoHeader()), _aacEncoder.IntWaveFormatEx, 2) == 0;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                { }
                VsNetRecordSdk.FileMuxer_FreeInstance(_handle);
                disposedValue = true;
            }
        }
        ~Record()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
