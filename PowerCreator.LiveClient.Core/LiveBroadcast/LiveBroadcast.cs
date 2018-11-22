using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DirectShowLib;
using PowerCreator.LiveClient.Core.AudioEncoder;
using PowerCreator.LiveClient.Core.Enums;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Core.VideoEncoder;
using PowerCreator.LiveClient.VsNetSdk;

namespace PowerCreator.LiveClient.Core.LiveBroadcast
{
    public class LiveBroadcast : ILiveBroadcast, IObserver<AudioEncodedDataContext>, IObserver<VideoEncodedDataContext>
    {
        public bool IsLive { get; private set; }
        public RecAndLiveState State { get; private set; }

        protected IDisposable unsubscriberVideoData;
        protected IDisposable unsubscriberAudioData;
        private readonly IntPtr _handle;
        private readonly IVideoEncoder _videoEncoder;
        private readonly IAacEncoder _aacEncoder;
        private string _liveServer;
        private int _liveServerPort;
        private string _liveChannel;
        private string _liveStreamName;
        private bool _isPushStreamSuccess;
        private int _continuousPushFailedCount = 0;
        public LiveBroadcast(IVideoEncoder videoEncoder, IAacEncoder aacEncoder)
        {
            State = RecAndLiveState.NotStart;
            _videoEncoder = videoEncoder;
            _aacEncoder = aacEncoder;

            _handle = VsNetRtmpSenderSdk.RtmpSender_AllocInstance();
            VsNetRtmpSenderSdk.RtmpSender_SendInThread(_handle, true);
        }

        public bool PauseLive()
        {
            throw new NotImplementedException();
        }

        public bool ResumeLive()
        {
            throw new NotImplementedException();
        }

        public bool StartLive(string liveServer, int liveServerPort, string liveChannel, string liveStreamName)
        {
            if (IsLive) return true;

            _liveServer = liveServer;
            _liveServerPort = liveServerPort;
            _liveChannel = liveChannel;
            _liveStreamName = liveStreamName;
            if (_startLive())
            {
                State = RecAndLiveState.Started;
                IsLive = true;
            }
            return false;
        }

        public bool StopLive()
        {
            if (!IsLive) return true;
            if (_stopLive())
            {
                State = RecAndLiveState.NotStart;
                return true;
            }
            return false;
        }
        private bool _stopLive()
        {
            //TODO  停止失败时会出现黑屏
            unsubscriberAudioData.Dispose();
            unsubscriberVideoData.Dispose();
            return VsNetRtmpSenderSdk.RtmpSender_EndWrite(_handle) == 0;
        }
        private bool _startLive()
        {
            _videoEncoder.StartVideoEncoder();
            _aacEncoder.StartAudioEncoder();
            if (_setLiveInfo())
            {
                unsubscriberVideoData = _videoEncoder.Subscribe(this);
                unsubscriberAudioData = _aacEncoder.Subscribe(this);
                return true;
            }
            return false;
        }
        private bool _setLiveInfo()
        {
            int r = VsNetRtmpSenderSdk.RtmpSender_BeginWrite(_handle, _liveServer, _liveServerPort, _liveChannel, _liveStreamName, _videoEncoder.IntBitmapInfoHeader, Marshal.SizeOf(new BitmapInfoHeader()), _aacEncoder.IntWaveFormatEx, Marshal.SizeOf(new WaveFormatEx()));
            return r == 0;
        }

        #region IObserver Support
        public void OnNext(VideoEncodedDataContext value)
        {
            if (value.DataLength <= 0) return;

            _isPushStreamSuccess = VsNetRtmpSenderSdk.RtmpSender_WriteVideo(_handle, value.Data, value.DataLength, value.KeyFrame, value.TimeStamp) == 0;
            if (_isPushStreamSuccess)
            {
                _continuousPushFailedCount = 0;
                return;
            }
            _continuousPushFailedCount++;
            if (_continuousPushFailedCount > 100)
            {
                while (true)
                {
                    _stopLive();

                    bool reStartSuccess = _startLive();
                    if (reStartSuccess)
                    {
                        //LogHelper.WriteLog(_name + "与服务器重连成功");
                        break;
                    }
                    else
                    {
                        //LogHelper.WriteLog(_name + "与服务器重连失败");
                        Thread.Sleep(1000);
                    }
                }
            }


        }
        public void OnNext(AudioEncodedDataContext value)
        {
            VsNetRtmpSenderSdk.RtmpSender_WriteAudio(_handle, value.Data.ToInt32(), value.DataLength, value.TimeStamp);
        }
        public void OnError(Exception error) { }
        public void OnCompleted() { }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) { }
                VsNetRtmpSenderSdk.RtmpSender_FreeInstance(_handle);
                disposedValue = true;
            }
        }
        ~LiveBroadcast()
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
