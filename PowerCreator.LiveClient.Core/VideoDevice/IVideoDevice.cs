﻿using PowerCreator.LiveClient.Core.Enums;
using PowerCreator.LiveClient.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.VideoDevice
{
    public interface IVideoDevice : IPushingDataPushingData<VideoDeviceDataContext>, IDisposable
    {
        int ID { get; }

        string Name { get; set; }

        int Width { get; }

        int Height { get; }

        VideoDeviceDataFormat Format { get; }

        bool IsOpen { get; }

        bool IsAvailable { get; }

        IntPtr DeviceBitmapInfoHeaderIntPtr { get; }
        int DeviceBitmapInfoHeader { get; }
        bool OpenDevice();

        bool CloseDevice();

    }
}
