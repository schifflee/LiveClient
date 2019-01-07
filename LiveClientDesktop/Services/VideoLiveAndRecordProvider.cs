using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.AudioEncoder;
using PowerCreator.LiveClient.Core.Enums;
using PowerCreator.LiveClient.Core.LiveBroadcast;
using PowerCreator.LiveClient.Core.Record;
using PowerCreator.LiveClient.Core.VideoDevice;
using PowerCreator.LiveClient.Core.VideoEncoder;
using PowerCreator.LiveClient.Log;
using System;
using System.IO;
using System.Linq;

namespace LiveClientDesktop.Services
{
    public abstract class VideoLiveAndRecordProvider
    {
        private IAudioDevice _audioDevice;
        private IVideoDevice _videoDevice;
        private IRecord _videoRecord;
        private ILiveBroadcast _videoLiveBroadcast;
        private IAacEncoder _aacEncoder;
        private IVideoEncoder _videoEncoder;
        private readonly SystemConfig _config;
        private readonly ILoggerFacade _logger;
        private readonly IAudioDeviceManager _audioDeviceManager;
        private string recFileName;
        protected abstract string Name { get; }
        public VideoLiveAndRecordProvider(SystemConfig config, ILoggerFacade logger, IAudioDeviceManager audioDeviceManager)
        {
            _config = config;
            _logger = logger;
            _aacEncoder = new AacEncoder();
            _videoEncoder = new H264VideoEncoder();
            _audioDeviceManager = audioDeviceManager;
            _audioDevice = _audioDeviceManager.GetAudioDeviceById(_config.UseMicrophoneID);
            if (_audioDevice == null)
            {
                _audioDevice = _audioDeviceManager.GetAudioDevices().First();
            }
            _aacEncoder.SetAudioDataSource(_audioDevice);
            _videoRecord = new Record(_videoEncoder, _aacEncoder);
            _videoLiveBroadcast = new LiveBroadcast(_videoEncoder, _aacEncoder);
        }

        #region 录制操作
        public Tuple<bool, string> StartRecording(int videoIndex)
        {
            Tuple<bool, string> result = Valid();
            if (!result.Item1) return result;

            recFileName = string.Format("{0:yyyyMMddHHmmss}-{1}", DateTime.Now, videoIndex);
            bool isSuccess = false;
            switch (_videoRecord.State)
            {
                case RecAndLiveState.NotStart:
                    isSuccess = _videoRecord.StartRecord(Path.Combine(_config.RecFileSavePath, recFileName));
                    return OperationResult(isSuccess, string.Format("{0}开始录制失败", Name));
                case RecAndLiveState.Pause:
                    isSuccess = _videoRecord.ResumeRecord();
                    return OperationResult(isSuccess, string.Format("{0}继续录制失败", Name));
            }
            return OperationResult(isSuccess, string.Format("{0}录制已经开启", Name));
        }
        public Tuple<bool, string> PauseRecording()
        {
            if (_videoRecord.State != RecAndLiveState.Started)
            {
                return OperationResult(false, string.Format("{0}暂停录制失败,当前未开启录制", Name));
            }

            bool isSuccess = _videoRecord.PauseRecord();
            return OperationResult(isSuccess, string.Format("{0}暂停录制失败", Name));

        }
        public Tuple<bool, string> StopRecording()
        {
            if (_videoRecord.State != RecAndLiveState.Started && _videoRecord.State != RecAndLiveState.Pause)
            {
                return OperationResult(false, string.Format("{0}停止录制失败,当前未开启录制", Name));
            }
            bool isSuccess = _videoRecord.StopRecord();
            return OperationResult(isSuccess, string.Format("{0}停止录制失败", Name));
        }
        #endregion

        #region 直播操作
        public Tuple<bool, string> StartLiving()
        {
            Tuple<bool, string> result = Valid();
            if (!result.Item1) return result;

            bool isSuccess = false;
            var liveStreamAddressInfo = GetliveStreamAddressInfo();
            if (liveStreamAddressInfo == null)
            {
                return OperationResult(isSuccess, "直播流推送地址获取失败");
            }
            if (_videoLiveBroadcast.State == RecAndLiveState.NotStart || _videoLiveBroadcast.State == RecAndLiveState.Pause)
            {
                isSuccess = _videoLiveBroadcast.StartLive(liveStreamAddressInfo.DominName, liveStreamAddressInfo.Port, liveStreamAddressInfo.AppName, liveStreamAddressInfo.StreamName);
                if (!isSuccess)
                {
                    _logger.Info(liveStreamAddressInfo.ToString());
                }
                return OperationResult(isSuccess, "直播流推送失败...");
            }
            return OperationResult(isSuccess, "当前正在直播");
        }
        public Tuple<bool, string> PauseLiving()
        {
            bool isSuccess = false;
            if (_videoLiveBroadcast.State != RecAndLiveState.Started)
            {
                return OperationResult(isSuccess, string.Format("{0}暂停失败,当前直播未开启!", Name));
            }
            isSuccess = _videoLiveBroadcast.PauseLive();
            return OperationResult(isSuccess, string.Format("{0}暂停直播失败", Name));
        }
        public Tuple<bool, string> StopLiving()
        {
            bool isSuccess = false;
            if (_videoLiveBroadcast.State != RecAndLiveState.Started && _videoLiveBroadcast.State != RecAndLiveState.Pause)
            {
                _logger.Info(string.Format("{0},State:{1}", Name, _videoLiveBroadcast.State));
                return OperationResult(isSuccess, string.Format("{0}停止失败,当前直播未开启!", Name));
            }
            isSuccess = _videoLiveBroadcast.StopLive();
            return OperationResult(isSuccess, string.Format("{0}停止直播失败", Name));
        }
        #endregion

        protected abstract LiveStreamAddressInfo GetliveStreamAddressInfo();

        private Tuple<bool, string> OperationResult(bool isSuccess, string errMsg = "")
        {
            if (isSuccess) return new Tuple<bool, string>(isSuccess, string.Empty);

            return new Tuple<bool, string>(isSuccess, errMsg);
        }
        private Tuple<bool, string> Valid()
        {
            if (_aacEncoder.CurrentUseAudioDevice == null)
                return OperationResult(false, "AudioDevice Cannot be null.");
            if (_videoEncoder.CurrentUseVideoDevice == null)
                return OperationResult(false, "VideoDevice Cannot be null.");
            return OperationResult(true);
        }
    }
}
