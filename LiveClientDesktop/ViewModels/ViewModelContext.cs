﻿using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class ViewModelContext
    {
        [Dependency]
        public CameraDeviceViewModel CameraDeviceViewModel
        {
            get; set;
        }
    }
}
