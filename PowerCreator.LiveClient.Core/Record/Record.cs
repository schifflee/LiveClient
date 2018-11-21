using System;
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
    public class Record : IRecord, IObserver<AudioEncodedDataContext>, IObserver<VideoEncodedDataContext>
    {
        public RecState RecState { get; private set; }

        public bool IsRecord { get; private set; }

        protected IDisposable unsubscriberVideoData;
        protected IDisposable unsubscriberAudioData;

        private readonly IntPtr _handle;
        private readonly IVideoEncoder _videoEncoder;
        private readonly IAacEncoder _aacEncoder;
        public Record(IVideoEncoder videoEncoder, IAacEncoder aacEncoder)
        {
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

            if (_setRecordInfo(recordFileSavePath))
            {
                unsubscriberVideoData = _videoEncoder.Subscribe(this);
                unsubscriberAudioData = _aacEncoder.Subscribe(this);
                RecState = RecState.Started;
                IsRecord = true;
                return true;
            }
            return false;
        }

        public bool StopRecord()
        {
            unsubscriberVideoData.Dispose();
            unsubscriberAudioData.Dispose();
            if (VsNetRecordSdk.FileMuxer_EndWrite(_handle) == 0)
            {
                RecState = RecState.NotStart;
                return true;
            }
            return false;
        }

        public bool PauseRecord()
        {
            throw new NotImplementedException();
        }

        public bool ResumeRecord()
        {
            throw new NotImplementedException();
        }

        private bool _setRecordInfo(string fileSavePath)
        {
            return VsNetRecordSdk.FileMuxer_BeginWrite(_handle, fileSavePath, _videoEncoder.IntBitmapInfoHeader, Marshal.SizeOf(new BitmapInfoHeader()), _aacEncoder.IntWaveFormatEx, 2) == 0;
        }

        #region IObserver Support

        public void OnNext(AudioEncodedDataContext value)
        {
            VsNetRecordSdk.FileMuxer_WriteAudio(_handle, value.Data.ToInt32(),value.DataLength ,value.KeyFrame);
        }
        public void OnNext(VideoEncodedDataContext value)
        {
            VsNetRecordSdk.FileMuxer_WriteVideo(_handle, value.Data, value.DataLength, value.KeyFrame, value.TimeStamp);
        }

        public void OnError(Exception error) { }

        public void OnCompleted()
        { }



        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Record() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion



    }
}
