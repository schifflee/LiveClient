using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Events;
using PowerCreator.LiveClient.Infrastructure;
using System;
using System.Linq;

namespace LiveClientDesktop
{
    public class SystemConfig
    {
        private const string _saveFileName = "config";
        private readonly IEventAggregator _eventAggregator;
        public readonly string AllDataSavePath;
        public readonly string TempImageSavePath;
        public SystemConfig(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AllDataSavePath = AppDomain.CurrentDomain.BaseDirectory + "Data";
            RecFileSavePath = AppDomain.CurrentDomain.BaseDirectory + "RecFiles";
            TempImageSavePath = AppDomain.CurrentDomain.BaseDirectory + "TempWindowImages";
            VideoVolume = 30;
            MicrophoneVolume = 50;
            UseMicrophoneID = 0;
            RecordingStatusChangesAccordingToLiveBroadcastStatus = false;
            UploadCompletedAutoDeleteLocalFile = false;
            IsAutoUpload = false;
        }

        public ResolutionInfo UseResolutionInfo { get; set; }
        public RateInfo UseRateInfo { get; set; }
        public FrameRateInfo UseFrameRateInfo { get; set; }
        public bool RecordingStatusChangesAccordingToLiveBroadcastStatus { get; set; }
        public bool UploadCompletedAutoDeleteLocalFile { get; set; }
        public string RecFileSavePath { get; set; }
        public int UseMicrophoneID { get; set; }
        public int VideoVolume { get; set; }
        public int MicrophoneVolume { get; set; }
        public int VideoIndex { get; set; }
        public bool IsAutoUpload { get; set; }

        public void Initialize()
        {
            var contents = FileHelper.ReadFileContent(AllDataSavePath, _saveFileName);
            if (!contents.Any()) return;

            var config = JsonHelper.DeserializeObject<SystemConfig>(FileHelper.ReadFileContent(AllDataSavePath, _saveFileName).First());
            if (config == null)
            {
                UseResolutionInfo = new ResolutionInfo { DisplayName = "1280*720", Height = 720, Width = 1280, ID = 1 };
                UseRateInfo = new RateInfo { ID = 3, DisplayName = "1500kbps", Value = 1500 };
                UseFrameRateInfo = new FrameRateInfo { ID = 1, DisplayName = "25Fps", Value = 25 };
                Save();
                return;
            }

            VideoIndex = 0;
            UseFrameRateInfo = config.UseFrameRateInfo;
            UseMicrophoneID = config.UseMicrophoneID;
            UseRateInfo = config.UseRateInfo;
            UseResolutionInfo = config.UseResolutionInfo;
            IsAutoUpload = config.IsAutoUpload;
            VideoVolume = config.VideoVolume;
            MicrophoneVolume = config.MicrophoneVolume;
            RecFileSavePath = config.RecFileSavePath;
            UploadCompletedAutoDeleteLocalFile = config.UploadCompletedAutoDeleteLocalFile;
            RecordingStatusChangesAccordingToLiveBroadcastStatus = config.RecordingStatusChangesAccordingToLiveBroadcastStatus;
        }
        public void Save()
        {
            try
            {
                FileHelper.DeleteFile(AllDataSavePath, _saveFileName);
                FileHelper.WriteAllLines(AllDataSavePath, _saveFileName, new string[] { JsonHelper.SerializeObject(this) });
                _eventAggregator.GetEvent<ConfigSaveEvent>().Publish(true);
            }
            catch { }
        }
    }
}
