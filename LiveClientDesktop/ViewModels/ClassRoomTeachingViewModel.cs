using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;

namespace LiveClientDesktop.ViewModels
{
    public class ClassRoomTeachingViewModel : NotificationObject
    {
        public DelegateCommand<string> ShowPanelCommand { get; set; }

        public ClassRoomTeachingViewModel(IEventAggregator eventAggregator)
        {
            ShowPanelCommand = new DelegateCommand<string>((windowType) =>
            {
                eventAggregator.GetEvent<ShowClassRoomTeachingWindowEvent>().Publish((ClassRoomTeachingWindowType)int.Parse(windowType));
            });
        }

    }


}
