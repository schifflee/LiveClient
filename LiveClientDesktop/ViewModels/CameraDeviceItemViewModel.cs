using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows;

namespace LiveClientDesktop.ViewModels
{
    public class CameraDeviceItemViewModel : NotificationObject
    {
        public CameraDeviceItemViewModel(VideoDeviceInfo videoDeviceInfo)
        {
            OwnerVideoDevice = videoDeviceInfo;
            IsSelected = false;
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                if (value)
                {
                    ShowSelectIcon = Visibility.Visible;
                    Foreground = "#188CFF";
                }
                else
                {
                    ShowSelectIcon = Visibility.Hidden;
                    Foreground = "#AAAAAA";
                }

                this.RaisePropertyChanged("IsSelected");
            }
        }
        private Visibility showSelectIcon;

        public Visibility ShowSelectIcon
        {
            get { return showSelectIcon; }
            set
            {
                showSelectIcon = value;
                this.RaisePropertyChanged("ShowSelectIcon");
            }
        }
        private string foreground;

        public string Foreground
        {
            get { return foreground; }
            set
            {
                foreground = value;
                this.RaisePropertyChanged("Foreground");
            }
        }


        public string Name => OwnerVideoDevice?.Name;
        public int ID { get { return OwnerVideoDevice.ID; } }
        public VideoDeviceInfo OwnerVideoDevice { get; private set; }
        public override string ToString()
        {
            return $"{Name},{ID}";
        }
    }
}
