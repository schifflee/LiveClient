using LiveClientDesktop.EventAggregations;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class ShellViewModel
    {
        [Dependency]
        public ViewModelContext VMContext
        {
            get; set;
        }
        [Dependency]
        public EventSubscriptionManager EventSubscriptionManager { get; set; }
    }
}
