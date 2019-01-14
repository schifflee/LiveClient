using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using PowerCreator.LiveClient.Infrastructure;
using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class UploadDocumentViewModel : NotificationObject
    {
        private readonly IServiceClient _serviceClient;
        private readonly LiveInfo _liveInfo;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;

        public DelegateCommand OpenSelectUploadFileWindow { get; set; }
        public UploadDocumentViewModel(IServiceClient serviceClient, LiveInfo liveInfo, WebPlatformApiFactory webPlatformApiFactory)
        {
            _serviceClient = serviceClient;
            _liveInfo = liveInfo;
            _webPlatformApiFactory = webPlatformApiFactory;


            OpenSelectUploadFileWindow = new DelegateCommand(new Action(UploadDocument));
        }
        private void UploadDocument()
        {
            OpenSelectFileDialog("PPT File(*.ppt;*.pptx;*.doc;*.docx;*.mp4;*.jpg;*.png;*.bmp;*.txt;*.zip;*.rar)|*.ppt;*.pptx;*.doc;*.docx;*.mp4;*.jpg;*.png;*.bmp;*.txt;*.zip;*.rar", (fileName) =>
            {
                try
                {
                    Task.Run(() =>
                    {
                        using (var fs = File.Open(fileName, FileMode.Open))
                        {
                            var request = _webPlatformApiFactory.CreateUploadDocumentRequest(
                                Path.GetFileName(fileName),
                                _liveInfo.TeacherList.Where(item => item.IsMajor).First().TeacherName,
                                Path.GetExtension(fileName),
                                fs);
                            request.StreamTransferProgress += StreamTransferProgress;
                            _serviceClient.GetResponse(request);
                        }

                    });
                }
                catch { }
            });
        }
        private void StreamTransferProgress(object sender, StreamTransferProgressArgs args)
        {

        }
        private void OpenSelectFileDialog(string fileFilter, Action<string> callback)
        {
            var selectedFileName = OpenFileDialogHelper.OpenFileDialogWindow(fileFilter);
            if (string.IsNullOrEmpty(selectedFileName)) return;
            callback?.Invoke(selectedFileName);
        }
    }
}
