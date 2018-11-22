using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.AudioEncoder;
using PowerCreator.LiveClient.Core.LiveBroadcast;
using PowerCreator.LiveClient.Core.VideoDevice;
using PowerCreator.LiveClient.Core.VideoEncoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        Task t;
        bool isRuning = true;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public Form1()
        {
            InitializeComponent();
            t = new Task(aa);
            cancellationTokenSource.Token.Register(() =>
            {


            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (t.IsCompleted)
            //    t.Dispose();
            //t = null;
            //if (t == null)
            //{
            //    t = new Task(aa, cancellationTokenSource.Token);
            //}
           // t.Start();
            msPlayControl1.OpenVideoDevice(0);
        }
        private void aa()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                Debug.WriteLine("1");
                Thread.Sleep(1000);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            msPlayControl1.CloseVideoDevice();
            //cancellationTokenSource.Cancel();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            var videoEncoder = new H264VideoEncoder();
            var videoDeviceManager =new VideoDeviceManager();
            var videoDevice = videoDeviceManager.GetVideoDeviceById(0);

            videoEncoder.SetVideoSource(videoDevice);

            var aacEncoder =new AacEncoder();
            var audioDeviceManager =new AudioDeviceManager();
            var audioDevice = audioDeviceManager.GetAudioDeviceById(1);

            aacEncoder.SetAudioDataSource(audioDevice);

            LiveBroadcast liveBroadcast = new LiveBroadcast(videoEncoder, aacEncoder);
            liveBroadcast.StartLive("192.168.0.202", 1935, "live", "test");
        }
    }
}
