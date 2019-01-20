using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.HttpRequestHandler;
using PowerCreator.LiveClient.Core.VideoDevice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiveClientDesktop.Models
{
    public class RuntimeState
    {
        public RuntimeState(EventSubscriptionManager eventSubscriptionManager)
        {
            HttpRequestHandleManager.Instance.AddHandler("GetLiveAndRecordState", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return HttpRequestHandleResultWarpper.WriteResult(true, null, this);
            }));
            eventSubscriptionManager.Subscribe<LiveAndRecordingOperateEvent, LiveAndRecordingOperateEventContext>(null, LiveAndRecordingOperateEventHandler, null);
            eventSubscriptionManager.Subscribe<LoadPresentationCompletedEvent, List<PresentationInfo>>(null, LoadPresentationCompletedEventHandler, null);
            eventSubscriptionManager.Subscribe<LoadWarmVideoCompletedEvent, List<PresentationInfo>>(null, LoadWarmVideoCompletedEventHandler, null);
            eventSubscriptionManager.Subscribe<LoadCameraDeviceCompletedEvent, List<IVideoDevice>>(null, LoadCameraDeviceCompletedEventHandler, null);
            eventSubscriptionManager.Subscribe<SwitchDemonstrationSceneEvent, SwitchDemonstrationSceneContext>(null, SwitchDemonstrationSceneEventHandler, null);
        }
        private void SwitchDemonstrationSceneEventHandler(SwitchDemonstrationSceneContext context)
        {
            ResetAllVideoSourceState();
            switch (context.Source)
            {
                case SceneType.VideoDevice:
                    var cameraDevice = CameraVideos.FirstOrDefault(item => item.ID == (context.UseDevice as IVideoDevice).ID);
                    if (cameraDevice != null) cameraDevice.IsActive = true;
                    break;
                case SceneType.PPT:
                    var p = ScreenVideos.FirstOrDefault(item => item.Name == Path.GetFileNameWithoutExtension(context.UseDevice.ToString()));
                    if (p != null) p.IsActive = true;
                    break;
                case SceneType.WarmVideo:
                    var w = StreamVideos.FirstOrDefault(item => item.Name == Path.GetFileNameWithoutExtension(context.UseDevice.ToString()));
                    if (w != null) w.IsRunning = true;
                    break;
            }
        }
        private void ResetAllVideoSourceState()
        {
            if (StreamVideos != null)
            {
                foreach (var item in StreamVideos)
                {
                    item.IsRunning = false;
                }
            }
            if (ScreenVideos != null)
            {
                foreach (var item in ScreenVideos)
                {
                    item.IsActive = false;
                }
            }
            if (CameraVideos != null)
            {
                foreach (var item in CameraVideos)
                {
                    item.IsActive = false;
                }
            }
        }
        private void LoadCameraDeviceCompletedEventHandler(List<IVideoDevice> data)
        {
            CameraVideos = data.Select(item => new CameraVideo
            {
                ID = item.ID,
                Name = item.Name
            }).ToList();
        }
        private void LoadWarmVideoCompletedEventHandler(List<PresentationInfo> data)
        {
            StreamVideos = data.Select(item => new WarmVideoInfo
            {
                ID = item.ID,
                Name = item.Name
            }).ToList();
        }
        private void LoadPresentationCompletedEventHandler(List<PresentationInfo> data)
        {
            ScreenVideos = data.Select(item => new ScreenVideo
            {
                ID = item.ID,
                Name = item.Name
            }).ToList();
        }
        private void LiveAndRecordingOperateEventHandler(LiveAndRecordingOperateEventContext context)
        {

            switch (context.EventType)
            {
                case LiveAndRecordingOperateEventType.Start:
                    Start(context.EventSource);
                    break;
                case LiveAndRecordingOperateEventType.Pause:
                    Pause(context.EventSource);
                    break;
                case LiveAndRecordingOperateEventType.Resume:
                    Start(context.EventSource);
                    break;
                case LiveAndRecordingOperateEventType.Stop:
                    Stop(context.EventSource);
                    break;
            }

        }

        private void Stop(LiveAndRecordingOperateEventSourceType type)
        {
            if (type == LiveAndRecordingOperateEventSourceType.Live)
            {
                IsLiving = false;
                IsLiveRunning = false;
            }
            else
            {
                IsRecording = false;
                IsRecordRunning = false;
            }
        }
        private void Pause(LiveAndRecordingOperateEventSourceType type)
        {
            Start(type);
            if (type == LiveAndRecordingOperateEventSourceType.Live)
                IsLiveRunning = false;
            else
                IsRecordRunning = false;
        }
        private void Start(LiveAndRecordingOperateEventSourceType type)
        {
            if (type == LiveAndRecordingOperateEventSourceType.Live)
            {
                IsLiving = true;
                IsLiveRunning = true;
            }
            else
            {
                IsRecording = true;
                IsRecordRunning = true;
            }

        }

        public List<WarmVideoInfo> StreamVideos { get; set; }

        public List<ScreenVideo> ScreenVideos { get; set; }

        public List<CameraVideo> CameraVideos { get; set; }

        public bool IsLiving { get; set; }

        public bool IsLiveRunning { get; set; }

        public bool IsRecording { get; set; }

        public bool IsRecordRunning { get; set; }
        public bool IsMute { get; set; }


    }
    public class WarmVideoInfo
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public bool IsAvaliable { get; set; }

        public bool IsRunning { get; set; }
    }

    public class ScreenVideo
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
    public class CameraVideo
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
