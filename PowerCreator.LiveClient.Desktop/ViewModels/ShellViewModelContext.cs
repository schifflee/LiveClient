using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Desktop.ViewModels
{
    public class ShellViewModelContext
    {
        public CameraDeviceViewModel CameraDeviceViewModel
        {
            get;
            private set;
        }
        public ShellViewModelContext()
        {
            CameraDeviceViewModel = new CameraDeviceViewModel(new Services.CameraDeviceService());
        }
    }
}
