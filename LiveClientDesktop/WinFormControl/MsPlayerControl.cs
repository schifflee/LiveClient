using System.Windows.Forms;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Core.VideoDevice;

namespace LiveClientDesktop.WinFormControl
{
    public partial class MsPlayerControl : UserControl
    {
        private object syncState = new object();
        private IVideoDevice _currentVideoDevice;
        public MsPlayerControl()
        {
            InitializeComponent();
        }

        public void OpenDevice(IVideoDevice videoDevice)
        {
            if (_currentVideoDevice != null)
            {
                _currentVideoDevice.PushingData -= _videoDevice_PushData;
            }
            _currentVideoDevice = videoDevice;
            _currentVideoDevice.PushingData += _videoDevice_PushData;
            PowerMsPlayer.StopDecData();
            PowerMsPlayer.StartInputDecData(0, videoDevice.DeviceBitmapInfoHeader);
        }



        private void _videoDevice_PushData(VideoDeviceDataContext value)
        {
            lock (syncState)
            {
                PowerMsPlayer.InputDecVideo(value.Data, value.DataLength);
            }
        }

        public void CloseDevice()
        {
            lock (syncState)
            {
                _currentVideoDevice.PushingData -= _videoDevice_PushData;
            }
        }
    }
}
