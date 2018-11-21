using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.Models
{
    public struct AudioDeviceDataContext
    {
        public int Data { get; }
        public int DataLength { get; }
        public AudioDeviceDataContext(int data, int dataLength)
        {
            Data = data;
            DataLength = dataLength;
        }
    }
}
