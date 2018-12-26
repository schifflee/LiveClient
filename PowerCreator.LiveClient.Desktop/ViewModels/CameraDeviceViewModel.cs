using Microsoft.Practices.Prism.ViewModel;

namespace PowerCreator.LiveClient.Desktop.ViewModels
{
    public class CameraDeviceViewModel : NotificationObject
    {
        public CameraDeviceViewModel()
        {
            foground = "#aaa";
        }
        private string foground;

        public string Foground
        {
            get { return foground; }
            set
            {
                foground = value;
                this.RaisePropertyChanged("Foground");
            }
        }


    }
}
