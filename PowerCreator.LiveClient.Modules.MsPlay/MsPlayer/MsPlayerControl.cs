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

namespace PowerCreator.LiveClient.Modules.MsPlayer.MsPlayer
{
    public partial class MsPlayerControl : UserControl, IObserver<VideoDeviceDataContext>
    {
        public MsPlayerControl()
        {
            InitializeComponent();
        }
        private IDisposable _unsubscriber;
        private IVideoDevice _currentUseVideoSource;
        private bool _isPlay;
        public bool Play(IVideoDevice videoSource)
        {
            if (_isPlay) return true;
            if (videoSource == null) return false;
            if (_currentUseVideoSource != null) _unsubscriber?.Dispose();

            _currentUseVideoSource = videoSource;
            _unsubscriber = _currentUseVideoSource.Subscribe(this);
            _currentUseVideoSource.OpenDevice();
            MSPlayer.StopDecData();
            MSPlayer.StartInputDecData(0, _currentUseVideoSource.DeviceBitmapInfoHeader);
            _isPlay = true;
            return true;
        }

        public bool Stop()
        {
            if (!_isPlay) return true;

            _unsubscriber?.Dispose();

            return true;
        }
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(VideoDeviceDataContext value)
        {
            MSPlayer.InputDecVideo(value.Data, value.DataLength);
        }
    }
}
