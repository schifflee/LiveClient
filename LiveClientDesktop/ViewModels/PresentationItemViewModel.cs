using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.ViewModel;

namespace LiveClientDesktop.ViewModels
{
    public class PresentationItemViewModel : NotificationObject
    {
        public PresentationInfo Presentation { get; set; }

        public override string ToString()
        {
            return Presentation.ToString();
        }
    }
}
