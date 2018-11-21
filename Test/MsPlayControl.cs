using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PowerCreator.LiveClient.Core.VideoDevice;
using PowerCreator.LiveClient.Core.Models;

namespace Test
{
    public partial class MsPlayControl : UserControl, IObserver<VideoDeviceDataContext>
    {
        public MsPlayControl()
        {
            InitializeComponent();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(VideoDeviceDataContext value)
        {
            axMSPlayer1.InputDecVideo(value.Data, value.DataLength);
        }
        private IDisposable unsubscriber;
        IVideoDevice videoDevice;
        VideoDeviceManager videoDeviceManager;
        public void OpenVideoDevice(int deviceId)
        {
             
            if (videoDeviceManager == null) {
                videoDeviceManager = new VideoDeviceManager();
            }
            videoDevice = videoDeviceManager.GetVideoDeviceById(0);
            unsubscriber = videoDevice.Subscribe(this);
            videoDevice.OpenDevice();
            axMSPlayer1.StopDecData();
            axMSPlayer1.StartInputDecData(0, videoDevice.DeviceBitmapInfoHeader);
        }
        public void CloseVideoDevice()
        {
            unsubscriber.Dispose();
            videoDevice.CloseDevice();

        }
    }
}
