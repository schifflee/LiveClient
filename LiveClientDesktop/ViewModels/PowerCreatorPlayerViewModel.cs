using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class PowerCreatorPlayerViewModel
    {
        [Dependency]
        public IEventAggregator EventAggregator { get; set; }
    }
}
