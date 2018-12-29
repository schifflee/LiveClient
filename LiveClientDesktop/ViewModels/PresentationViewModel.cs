using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using PowerCreator.LiveClient.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class PresentationViewModel : NotificationObject
    {
        protected readonly PresentationsRepository _presentationsRepository;

        public DelegateCommand OpenSelectFileWindow { get; set; }
        public PresentationViewModel(PresentationsRepository presentationsRepository)
        {
            _presentationsRepository = presentationsRepository ?? throw new ArgumentNullException("presentationsRepository");
            OpenSelectFileWindow = new DelegateCommand(OpenSelectFileDialog);
        }
        private void OpenSelectFileDialog()
        {
            var selectedFileName = OpenFileDialogHelper.OpenFileDialogWindow("PPT File(*.ppt;*.pptx;*.avi;*.mp4;*.mkv;*.wav;*.flv;*.asf;)|*.ppt;*.pptx;*.avi;*.mp4;*.mkv;*.wav;*.flv;*.asf;");

        }
    }
}
