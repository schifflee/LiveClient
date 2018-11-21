using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.Models
{
    public struct AudioEncodedDataContext
    {
        public IntPtr Data;
        public int DataLength, TimeStamp;
        public bool KeyFrame;
        public AudioEncodedDataContext(IntPtr data, int dataLength, int timeStamp, bool keyFrame)
        {
            Data = data;
            DataLength = dataLength;
            TimeStamp = timeStamp;
            KeyFrame = keyFrame;
        }
    }
}
