﻿using Microsoft.Practices.Prism.Logging;
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

namespace PowerCreator.LiveClient.Core
{
    public abstract class VideoLiveAndRecordProvider : ISetupVideoLiveAndRecordingDevices, IVideoLiveProvider, IVideoRecordingProvider
    {
        private IAudioDevice _useAudioDevice;
        private IVideoDevice _useVideoDevice;
        private IRecord _videoRecord;
        private ILiveBroadcast _videoLiveBroadcast;
        private IAacEncoder _aacEncoder;
        private IVideoEncoder _videoEncoder;
        private readonly ILoggerFacade _logger;
        private readonly IAudioDeviceManager _audioDeviceManager;
        private string _recFileName;
        protected abstract string Name { get; }
        public RecAndLiveState LiveState
        {
            get
            {
                if (_videoLiveBroadcast != null) return _videoLiveBroadcast.State;
                return RecAndLiveState.NotStart;
            }
        }
        public RecAndLiveState RecordState
        {
            get
            {
                if (_videoRecord != null) return _videoRecord.State;
                return RecAndLiveState.NotStart;
            }
        }
        public VideoLiveAndRecordProvider(ILoggerFacade logger, IAudioDeviceManager audioDeviceManager, int defaultMicrophoneID)
        {
            _logger = logger;
            _aacEncoder = new AacEncoder();
            _videoEncoder = new H264VideoEncoder();
            _audioDeviceManager = audioDeviceManager;
            _useAudioDevice = _audioDeviceManager.GetAudioDeviceById(defaultMicrophoneID);
            if (_useAudioDevice == null)
            {
                _useAudioDevice = _audioDeviceManager.GetAudioDevices().First();
            }
            _aacEncoder.SetAudioDataSource(_useAudioDevice);
            _videoRecord = new Record.Record(_videoEncoder, _aacEncoder);
            _videoLiveBroadcast = new LiveBroadcast.LiveBroadcast(_videoEncoder, _aacEncoder);
        }

        #region 录制操作
        public Tuple<bool, string> StartRecording(string recFileSavePath, int videoIndex = 0)
        {
            Tuple<bool, string> result = Valid();
            if (!result.Item1) return result;

            _recFileName = string.Format("{0}{1:yyyyMMddHHmmss}-{2}.mp4", Name, DateTime.Now, videoIndex);
            bool isSuccess = false;
            switch (_videoRecord.State)
            {
                case RecAndLiveState.NotStart:

                    if (!Directory.Exists(recFileSavePath))
                        Directory.CreateDirectory(recFileSavePath);

                    isSuccess = _videoRecord.StartRecord(Path.Combine(recFileSavePath, _recFileName));
                    return OperationResult(isSuccess, $"{Name}开始录制失败");
                case RecAndLiveState.Pause:
                    isSuccess = _videoRecord.ResumeRecord();
                    return OperationResult(isSuccess, $"{Name}继续录制失败");
            }
            return OperationResult(isSuccess, $"{Name}录制已经开启");
        }
        public Tuple<bool, string> PauseRecording()
        {
            if (_videoRecord.State != RecAndLiveState.Started)
            {
                return OperationResult(false, $"{Name}暂停录制失败,当前未开启录制");
            }

            bool isSuccess = _videoRecord.PauseRecord();
            return OperationResult(isSuccess, $"{Name}暂停录制失败");

        }
        public Tuple<bool, string> StopRecording()
        {
            if (_videoRecord.State != RecAndLiveState.Started && _videoRecord.State != RecAndLiveState.Pause)
            {
                return OperationResult(false, $"{Name}停止录制失败,当前未开启录制");
            }
            bool isSuccess = _videoRecord.StopRecord();
            return OperationResult(isSuccess, $"{Name}停止录制失败");
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
                return OperationResult(isSuccess, $"{Name}直播流推送地址获取失败");
            }
            if (_videoLiveBroadcast.State == RecAndLiveState.NotStart || _videoLiveBroadcast.State == RecAndLiveState.Pause)
            {
                isSuccess = _videoLiveBroadcast.StartLive(liveStreamAddressInfo.DominName, liveStreamAddressInfo.Port, liveStreamAddressInfo.AppName, liveStreamAddressInfo.StreamName);
                if (!isSuccess)
                {
                    _logger.Info(liveStreamAddressInfo.ToString());
                }
                return OperationResult(isSuccess, $"{Name}直播流推送失败...");
            }
            return OperationResult(isSuccess, $"{Name}当前正在直播");
        }
        public Tuple<bool, string> PauseLiving()
        {
            bool isSuccess = false;
            if (_videoLiveBroadcast.State != RecAndLiveState.Started)
            {
                return OperationResult(isSuccess, $"{Name}暂停失败,当前直播未开启!");
            }
            isSuccess = _videoLiveBroadcast.PauseLive();
            return OperationResult(isSuccess, $"{Name}暂停直播失败");
        }
        public Tuple<bool, string> StopLiving()
        {
            bool isSuccess = false;
            if (_videoLiveBroadcast.State != RecAndLiveState.Started && _videoLiveBroadcast.State != RecAndLiveState.Pause)
            {
                _logger.Info($"{Name},State:{_videoLiveBroadcast.State}");
                return OperationResult(isSuccess, $"{Name}停止失败,当前直播未开启!");
            }
            isSuccess = _videoLiveBroadcast.StopLive();
            return OperationResult(isSuccess, $"{Name}停止直播失败");
        }
        #endregion

        public bool SetVideoDevice(IVideoDevice videoDevice)
        {
            if (_useVideoDevice == videoDevice) return true;
            _useVideoDevice = videoDevice;
            return _videoEncoder.SetVideoSource(_useVideoDevice);
        }
        public bool SetAudioDevice(IAudioDevice audioDevice)
        {
            if (_useAudioDevice == audioDevice) return true;
            _useAudioDevice = audioDevice;
            return _aacEncoder.SetAudioDataSource(_useAudioDevice);
        }
        protected abstract ILiveStreamAddressInfo GetliveStreamAddressInfo();

        private Tuple<bool, string> OperationResult(bool isSuccess, string errMsg = "")
        {
            if (isSuccess) return new Tuple<bool, string>(isSuccess, string.Empty);

            return new Tuple<bool, string>(isSuccess, errMsg);
        }
        private Tuple<bool, string> Valid()
        {
            if (_useAudioDevice == null)
                return OperationResult(false, "AudioDevice Cannot be null.");
            if (_useVideoDevice == null)
                return OperationResult(false, "VideoDevice Cannot be null.");
            return OperationResult(true);
        }
    }
}