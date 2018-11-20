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
    public partial class MsPlayControl : UserControl, IObserver<VideoDeviceData>
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

        public void OnNext(VideoDeviceData value)
        {
            MessageBox.Show(string.Format("{0}:{1}", value.Data, value.DataLength));
        }

        public void OpenCamera(int id)
        {
            VideoDeviceManager videoDeviceManager = new VideoDeviceManager();
            var s = videoDeviceManager.GetVideoDeviceById(0);
            s.Subscribe(this);
            s.OpenDevice();
        }
       
    }
}
