using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Core.VideoDevice;

namespace LiveClientDesktop.WinFormControl
{
    public partial class MsPlayerControl : UserControl, IObserver<VideoDeviceDataContext>
    {
        protected IDisposable unsubscriber;
        private object syncState = new object();
        private readonly string identity;
        public MsPlayerControl()
        {
            InitializeComponent();
            identity = Guid.NewGuid().ToString();
        }

        public void OpenDevice(IVideoDevice videoDevice)
        {
            unsubscriber?.Dispose();
            unsubscriber = null;
            videoDevice.OpenDevice();
            unsubscriber = videoDevice.Subscribe(this);
            PowerMsPlayer.StopDecData();
            PowerMsPlayer.StartInputDecData(0, videoDevice.DeviceBitmapInfoHeader);
        }
        public void CloseDevice()
        {
            lock (syncState)
            {
                unsubscriber?.Dispose();
            }
        }
        public void OnNext(VideoDeviceDataContext value)
        {
            lock (syncState) {
                PowerMsPlayer.InputDecVideo(value.Data, value.DataLength);
            }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}
