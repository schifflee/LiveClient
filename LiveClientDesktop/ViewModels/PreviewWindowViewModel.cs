using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class PreviewWindowViewModel : NotificationObject
    {

        private ObservableCollection<PreviewWindowInfo> windowList;

        public ObservableCollection<PreviewWindowInfo> WindowList
        {
            get { return windowList; }
            set
            {
                windowList = value;
                this.RaisePropertyChanged("WindowList");
            }
        }

        private PreviewWindowInfo selectedWindow;

        public PreviewWindowInfo SelectedWindow
        {
            get { return selectedWindow; }
            set
            {
                selectedWindow = value;
                this.RaisePropertyChanged("SelectedWindow");
            }
        }
        private readonly IEventAggregator _eventAggregator;
        private readonly EnumerationWindowService _enumerationWindowService;
        private bool isLoadPreviewWindow;
        public PreviewWindowViewModel(IEventAggregator eventAggregator, EnumerationWindowService enumerationWindowService)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException("eventAggregator");
            _enumerationWindowService = enumerationWindowService ?? throw new ArgumentNullException("enumerationWindowService");
        }

        public void LoadPreviewWindowImages()
        {
            if (isLoadPreviewWindow)
                return;

            isLoadPreviewWindow = true;
            WindowList = _enumerationWindowService.GetWindowList();
            if (WindowList != null)
            {
                SelectedWindow = WindowList.FirstOrDefault();
            }
            isLoadPreviewWindow = false;
        }
        public void ClearPreviewImage()
        {
            _enumerationWindowService.ClearPreviewImage();
        }
        public void PublishSelectedDemonstrationWindowEvent()
        {
            if (SelectedWindow != null)
                _eventAggregator.GetEvent<SelectedDemonstrationWindowEvent>().Publish(SelectedWindow);
        }
    }
}
