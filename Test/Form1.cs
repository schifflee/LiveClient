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
            if (t.IsCompleted)
                t.Dispose();
            t = null;
            if (t == null)
            {
                t = new Task(aa, cancellationTokenSource.Token);
            }
            t.Start();
            //msPlayControl1.OpenCamera(0);
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
            cancellationTokenSource.Cancel();
        }
    }
}
