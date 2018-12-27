using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Desktop.ViewModels
{
    public class ShellViewModel
    {
        public ShellViewModelContext VmContext
        {
           private set;
            get;
        }
        public ShellViewModel()
        {
            VmContext = new ShellViewModelContext();
        }
    }
}
