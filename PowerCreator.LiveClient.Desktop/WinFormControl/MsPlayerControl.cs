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

namespace PowerCreator.LiveClient.Desktop.WinFormControl
{
    public partial class MsPlayerControl : UserControl, IObserver<VideoDeviceDataContext>
    {
        protected IDisposable unsubscriber;
        public MsPlayerControl()
        {
            InitializeComponent();
        }

        public void OpenDevice(IVideoDevice videoDevice)
        {
            unsubscriber?.Dispose();
            unsubscriber = videoDevice.Subscribe(this);
            MsPlayer.StopDecData();
            MsPlayer.StartInputDecData(0, videoDevice.DeviceBitmapInfoHeader);
        }
        public void CloseDevice()
        {
            unsubscriber?.Dispose();
        }
        public void OnNext(VideoDeviceDataContext value)
        {
            MsPlayer.InputDecVideo(value.Data, value.DataLength);
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }


    }
}
