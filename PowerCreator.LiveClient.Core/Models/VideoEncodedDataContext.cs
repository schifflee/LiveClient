using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.Models
{
    public class VideoEncodedDataContext
    {
        public int Data, DataLength, TimeStamp;
        public bool KeyFrame;
        public VideoEncodedDataContext(int data, int dataLength, int timeStamp, bool keyFrame)
        {
            Data = data;
            DataLength = dataLength;
            TimeStamp = timeStamp;
            KeyFrame = keyFrame;
        }
    }
}
