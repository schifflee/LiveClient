using PowerCreator.LiveClient.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class VideoDeviceDataReceive : IObserver<VideoDeviceData>
    {
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
           
        }
    }
}
