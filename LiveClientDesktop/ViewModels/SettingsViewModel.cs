using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using NAudio.Wave;
using PowerCreator.LiveClient.Core.AudioDevice;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LiveClientDesktop.ViewModels
{
    public class SettingsViewModel : NotificationObject
    {
        private readonly SystemConfig _config;
        private readonly IAudioDeviceManager _audioDeviceManager;
        private readonly CameraDeviceService _cameraDeviceService;
        private readonly VideoDeviceAliasService _videoDeviceAliasService;

        public DelegateCommand ChangeRecordFileSaveFolderCommand;
        public DelegateCommand OpenRecordFileSaveFolderCommand;
        public List<ResolutionInfo> ResolutionInfoList { get; set; }
        public List<RateInfo> RateInfoList { get; set; }
        public List<FrameRateInfo> FrameRateInfoList { get; set; }
        public IEnumerable<IAudioDevice> AduioDeviceList { get; set; }
        public List<SpeakerInfo> SpeakerList { get; set; }

        public List<VideoDeviceAlias> VideoDeviceAliasList { get; set; }
        public IEnumerable<VideoDeviceInfo> VideoDeviceList { get; set; }

        private ResolutionInfo selectedResolutionInfo;

        public ResolutionInfo SelectedResolutionInfo
        {
            get { return selectedResolutionInfo; }
            set
            {
                selectedResolutionInfo = value;
                this.RaisePropertyChanged("SelectedResolutionInfo");
            }
        }
        private FrameRateInfo selectedFrameRateInfo;

        public FrameRateInfo SelectedFrameRateInfo
        {
            get { return selectedFrameRateInfo; }
            set
            {
                selectedFrameRateInfo = value;
                this.RaisePropertyChanged("SelectedFrameRateInfo");
            }
        }
        private RateInfo selectedRateInfo;

        public RateInfo SelectedRateInfo
        {
            get { return selectedRateInfo; }
            set
            {
                selectedRateInfo = value;
                this.RaisePropertyChanged("SelectedRateInfo");
            }
        }

        private IAudioDevice selectedAudioDevice;

        public IAudioDevice SelectedAudioDevice
        {
            get { return selectedAudioDevice; }
            set
            {
                selectedAudioDevice = value;
                this.RaisePropertyChanged("SelectedAudioDevice");
            }
        }
        private IAudioDevice debugAduioDevice;

        public IAudioDevice DebugAduioDevice
        {
            get { return debugAduioDevice; }
            set
            {
                debugAduioDevice = value;
                this.RaisePropertyChanged("DebugAduioDevice");
            }
        }

        private SpeakerInfo selectedSpeaker;

        public SpeakerInfo SelectedSpeaker
        {
            get { return selectedSpeaker; }
            set
            {
                selectedSpeaker = value;
                this.RaisePropertyChanged("SelectedSpeaker");
            }
        }

        private VideoDeviceInfo selectedVideoDevice;

        public VideoDeviceInfo SelectedVideoDevice
        {
            get { return selectedVideoDevice; }
            set
            {
                selectedVideoDevice = value;
                this.RaisePropertyChanged("SelectedVideoDevice");
            }
        }


        private string recFileSavePath;

        public string RecFileSavePath
        {
            get { return recFileSavePath; }
            set
            {
                recFileSavePath = value;
                this.RaisePropertyChanged("RecFileSavePath");
            }
        }

        private bool recordingStatusChangesAccordingToLiveBroadcastStatus;

        public bool RecordingStatusChangesAccordingToLiveBroadcastStatus
        {
            get { return recordingStatusChangesAccordingToLiveBroadcastStatus; }
            set
            {
                recordingStatusChangesAccordingToLiveBroadcastStatus = value;
                this.RaisePropertyChanged("RecordingStatusChangesAccordingToLiveBroadcastStatus");
            }
        }


        private bool uploadCompletedAutoDeleteLocalFile;

        public bool UploadCompletedAutoDeleteLocalFile
        {
            get { return uploadCompletedAutoDeleteLocalFile; }
            set
            {
                uploadCompletedAutoDeleteLocalFile = value;
                this.RaisePropertyChanged("UploadCompletedAutoDeleteLocalFile");
            }
        }

        private bool autoUpload;

        public bool AutoUpload
        {
            get { return autoUpload; }
            set
            {
                autoUpload = value;
                this.RaisePropertyChanged("AutoUpload");
            }
        }


        private bool saveBtnIsEnable;

        public bool SaveBtnIsEnable
        {
            get { return saveBtnIsEnable; }
            set
            {
                saveBtnIsEnable = value;
                this.RaisePropertyChanged("SaveBtnIsEnable");
            }
        }



        public SettingsViewModel(SystemConfig config,
            IAudioDeviceManager audioDeviceManager,
            CameraDeviceService cameraDeviceService,
            VideoDeviceAliasService videoDeviceAliasService)
        {
            _config = config;
            _audioDeviceManager = audioDeviceManager;
            _cameraDeviceService = cameraDeviceService;
            _videoDeviceAliasService = videoDeviceAliasService;
        }
        public void Initialize()
        {
            ResolutionInfoList = new List<ResolutionInfo> {
                new ResolutionInfo{ID=1,DisplayName="1280*720",Width=1280,Height=720 },
                new ResolutionInfo{ ID=2,DisplayName="960*540",Width=960,Height=540},
                new ResolutionInfo{ ID=3,DisplayName="640*360",Width=640,Height=360},
                new ResolutionInfo{ ID=4,DisplayName="480*270",Width=480,Height=270},
            };
            RateInfoList = new List<RateInfo>
            {
                    new RateInfo{ ID=2,DisplayName="1000kbps",Value=1000},
                    new RateInfo{ ID=3,DisplayName="1500kbps",Value=1500},
                    new RateInfo{ ID=4,DisplayName="2000kbps",Value=2000},
                    new RateInfo{ ID=5,DisplayName="2500kbps",Value=2500},
                    new RateInfo{ ID=6,DisplayName="3000kbps",Value=3000},
                    new RateInfo{ ID=7,DisplayName="4000kbps",Value=4000},
                    new RateInfo{ ID=8,DisplayName="5000kbps",Value=5000},
                    new RateInfo{ ID=9,DisplayName="8000kbps",Value=8000}
            };
            FrameRateInfoList = new List<FrameRateInfo>
            {
                    new FrameRateInfo(){ID=1,DisplayName="20FPS",Value=20 },
                    new FrameRateInfo(){ID=2,DisplayName="25FPS",Value=25 },
                    new FrameRateInfo(){ ID=3,DisplayName="30FPS",Value=30}
            };
            if (WaveOut.DeviceCount > 0)
            {
                SpeakerList = new List<SpeakerInfo>();
                for (var deviceId = -1; deviceId < WaveOut.DeviceCount; deviceId++)
                {
                    var capabilities = WaveOut.GetCapabilities(deviceId);
                    SpeakerList.Add(new SpeakerInfo { ID = deviceId, Name = capabilities.ProductName }); //$"Device {deviceId} ({capabilities.ProductName})");
                }
                SelectedSpeaker = SpeakerList.First();
            }
            VideoDeviceList = _cameraDeviceService.GetVideoDevices();

            //TODO 对已移除的设备的备注名称需要做移除处理
            VideoDeviceAliasList = _videoDeviceAliasService.GetDeviceAliasList();
            foreach (var item in VideoDeviceList)
            {
                if (!VideoDeviceAliasList.Any(d => d.DeviceName == item.Name))
                    VideoDeviceAliasList.Add(new VideoDeviceAlias
                    {
                        DeviceName = item.Name,
                        DeviceNoteName = item.Name
                    });
            }
            SelectedVideoDevice = VideoDeviceList.FirstOrDefault();


            AduioDeviceList = _audioDeviceManager.GetAudioDevices();
            DebugAduioDevice = AduioDeviceList.First();
            SelectedAudioDevice = AduioDeviceList.FirstOrDefault(item => item.ID == _config.UseMicrophoneID);
            SelectedResolutionInfo = ResolutionInfoList.FirstOrDefault(item => item.ID == _config.UseResolutionInfo.ID);
            SelectedFrameRateInfo = FrameRateInfoList.FirstOrDefault(item => item.ID == _config.UseFrameRateInfo.ID);
            SelectedRateInfo = RateInfoList.FirstOrDefault(item => item.ID == _config.UseRateInfo.ID);
            RecFileSavePath = _config.RecFileSavePath;
            RecordingStatusChangesAccordingToLiveBroadcastStatus = _config.RecordingStatusChangesAccordingToLiveBroadcastStatus;
            UploadCompletedAutoDeleteLocalFile = _config.UploadCompletedAutoDeleteLocalFile;
            AutoUpload = _config.IsAutoUpload;
            SaveBtnIsEnable = true;
        }
        public void Save()
        {
            _videoDeviceAliasService.Save(VideoDeviceAliasList);
            _config.IsAutoUpload = AutoUpload;
            _config.RecFileSavePath = RecFileSavePath;
            _config.RecordingStatusChangesAccordingToLiveBroadcastStatus = RecordingStatusChangesAccordingToLiveBroadcastStatus;
            _config.UploadCompletedAutoDeleteLocalFile = UploadCompletedAutoDeleteLocalFile;
            _config.UseFrameRateInfo = SelectedFrameRateInfo;
            _config.UseResolutionInfo = SelectedResolutionInfo;
            _config.UseRateInfo = SelectedRateInfo;
            _config.UseMicrophoneID = SelectedAudioDevice.ID;
            _config.Save();
        }
    }
}
