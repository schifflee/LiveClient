using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.ViewModel;
using System.Linq;

namespace LiveClientDesktop.ViewModels
{
    public class WelcomeViewModel : NotificationObject
    {
        public WelcomeViewModel(LiveInfo liveInfo)
        {
            if (liveInfo.TeacherList != null)
            {

                liveInfo.TeacherList
                     .Where(item => item.IsMajor)
                     .ToList()
                     .ForEach(item =>
                     {
                         if (!string.IsNullOrEmpty(_teachersNames))
                         {
                             _teachersNames += ",";
                         }
                         _teachersNames += $"{item.TeacherName}老师";
                     });
                TeachersNames = _teachersNames;
                Title = liveInfo.Title;
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.RaisePropertyChanged("Title");
            }
        }

        private string _teachersNames;
        public string TeachersNames
        {
            get { return _teachersNames; }
            set
            {
                _teachersNames = value;
                this.RaisePropertyChanged("TeachersNames");
            }
        }
    }
}
