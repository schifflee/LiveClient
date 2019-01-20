using System;
using System.Runtime.InteropServices;
using System.Threading;
using DirectShowLib;
using PowerCreator.LiveClient.Core.AudioEncoder;
using PowerCreator.LiveClient.Core.Enums;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Core.VideoEncoder;
using PowerCreator.LiveClient.VsNetSdk;

namespace PowerCreator.LiveClient.Core.LiveBroadcast
{
    internal class LiveBroadcast : ILiveBroadcast
    {
        public bool IsLive { get; private set; }
        public RecAndLiveState State { get; private set; }

        private readonly IntPtr _handle;
        private readonly IVideoEncoder _videoEncoder;
        private readonly IAacEncoder _aacEncoder;
        private string _liveServer;
        private int _liveServerPort;
        private string _liveChannel;
        private string _liveStreamName;
        private bool _isPushStreamSuccess;
        private int _continuousPushFailedCount = 0;

        private event Action _onNetworkInterruption;
        public event Action OnNetworkInterruption
        {
            add
            {
                _onNetworkInterruption -= value;
                _onNetworkInterruption += value;
            }
            remove
            {
                _onNetworkInterruption -= value;
            }
        }
        private event Action _onNetworkReconnectionSucceeded;
        public event Action OnNetworkReconnectionSucceeded
        {
            add
            {
                _onNetworkReconnectionSucceeded -= value;
                _onNetworkReconnectionSucceeded += value;
            }
            remove
            {
                _onNetworkReconnectionSucceeded -= value;
            }
        }
        private event Action _onNetworkReconnectionFailed;
        public event Action OnNetworkReconnectionFailed
        {
            add
            {
                _onNetworkReconnectionFailed -= value;
                _onNetworkReconnectionFailed += value;
            }
            remove
            {
                _onNetworkReconnectionFailed -= value;
            }
        }
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
            _videoEncoder.PushingData -= VideoEncoderPushingData;
            _aacEncoder.PushingData -= AACEncoderPushingData;
            bool success = VsNetRtmpSenderSdk.RtmpSender_EndWrite(_handle) == 0;
            if (success)
            {
                State = RecAndLiveState.Pause;
                IsLive = false;
            }
            else
            {
                _videoEncoder.PushingData += VideoEncoderPushingData;
                _aacEncoder.PushingData += AACEncoderPushingData;
            }
            return success;
        }

        public bool ResumeLive()
        {
            bool success = VsNetRtmpSenderSdk.RtmpSender_Resume(_handle) == 0;
            if (success)
            {
                State = RecAndLiveState.Started;
            }
            return success;
        }

        public bool StartLive(string liveServer, int liveServerPort, string liveChannel, string liveStreamName)
        {
            if (IsLive) return true;

            _liveServer = liveServer;
            _liveServerPort = liveServerPort;
            _liveChannel = liveChannel;
            _liveStreamName = liveStreamName;
            if (StartLive())
            {
                State = RecAndLiveState.Started;
                IsLive = true;
                return true;
            }
            return false;
        }

        public bool StopLive()
        {
            if (!IsLive) return true;
            if (_StopLive())
            {
                State = RecAndLiveState.NotStart;
                IsLive = false;
                return true;
            }
            return false;
        }
        private bool _StopLive()
        {
            //TODO  停止失败时会出现黑屏

            _videoEncoder.PushingData -= VideoEncoderPushingData;
            _aacEncoder.PushingData -= AACEncoderPushingData;
            return VsNetRtmpSenderSdk.RtmpSender_EndWrite(_handle) == 0;
        }
        private bool StartLive()
        {
            _videoEncoder.StartVideoEncoder();
            _aacEncoder.StartAudioEncoder();
            if (SetLiveInfo())
            {
                _videoEncoder.PushingData += VideoEncoderPushingData;
                _aacEncoder.PushingData += AACEncoderPushingData;
                return true;
            }
            return false;
        }

        private void AACEncoderPushingData(AudioEncodedDataContext value)
        {
            VsNetRtmpSenderSdk.RtmpSender_WriteAudio(_handle, value.Data.ToInt32(), value.DataLength, value.TimeStamp);
        }

        private void VideoEncoderPushingData(VideoEncodedDataContext value)
        {
            if (value.DataLength <= 0) return;

            _isPushStreamSuccess = VsNetRtmpSenderSdk.RtmpSender_WriteVideo(_handle, value.Data, value.DataLength, value.KeyFrame, value.TimeStamp) == 0;
            if (_isPushStreamSuccess)
            {
                _continuousPushFailedCount = 0;
                return;
            }
            if (_continuousPushFailedCount == 0)
            {
                _onNetworkInterruption?.Invoke();
            }
            _continuousPushFailedCount++;
            if (_continuousPushFailedCount > 100)
            {
                while (true)
                {
                    _StopLive();

                    bool reStartSuccess = StartLive();
                    if (reStartSuccess)
                    {
                        _onNetworkReconnectionSucceeded?.Invoke();
                        break;
                    }
                    else
                    {
                        _onNetworkReconnectionFailed?.Invoke();
                        Thread.Sleep(5000);
                    }
                }
            }

        }

        private bool SetLiveInfo()
        {
            int r = VsNetRtmpSenderSdk.RtmpSender_BeginWrite(_handle, _liveServer, _liveServerPort, _liveChannel, _liveStreamName, _videoEncoder.IntBitmapInfoHeader, Marshal.SizeOf(new BitmapInfoHeader()), _aacEncoder.IntWaveFormatEx, Marshal.SizeOf(new WaveFormatEx()));
            return r == 0;
        }

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
