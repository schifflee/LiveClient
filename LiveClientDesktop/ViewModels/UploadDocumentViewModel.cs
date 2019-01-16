using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Win32;
using PowerCreator.LiveClient.Infrastructure;
using PowerCreator.LiveClient.Log;
using PowerCreatorDotCom.Sdk.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LiveClientDesktop.ViewModels
{
    public class UploadDocumentViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private readonly DocumentService _documentService;
        private readonly string _fileFilter;
        private readonly Timer _timer;
        private bool _isUploading;
        public DelegateCommand OpenSelectUploadFileWindow { get; set; }

        public DelegateCommand<int?> DeleteDocumentCommand { get; set; }
        public DelegateCommand<int?> DownloadDocumentCommand { get; set; }

        private double uplodingProgressValue;

        public double UplodingProgressValue
        {
            get { return uplodingProgressValue; }
            set
            {
                uplodingProgressValue = value;
                this.RaisePropertyChanged("UplodingProgressValue");
            }
        }
        private bool isUploadCompleted;

        public bool IsUploadCompleted
        {
            get { return isUploadCompleted; }
            set
            {
                isUploadCompleted = value;
                this.RaisePropertyChanged("IsUploadCompleted");
            }
        }
        private ObservableCollection<DocumentItemViewModel> documents;

        public ObservableCollection<DocumentItemViewModel> Documents
        {
            get { return documents; }
            set
            {
                documents = value;
                this.RaisePropertyChanged("Documents");
            }
        }


        public UploadDocumentViewModel(ILoggerFacade logger, DocumentService documentService, string fileFilter)
        {
            _fileFilter = fileFilter;
            _logger = logger;
            _documentService = documentService;

            Documents = new ObservableCollection<DocumentItemViewModel>();
            OpenSelectUploadFileWindow = new DelegateCommand(new Action(UploadDocument));
            DeleteDocumentCommand = new DelegateCommand<int?>(new Action<int?>(DeleteDocumentById));
            DownloadDocumentCommand = new DelegateCommand<int?>(new Action<int?>(DownloadDocumentById));
            _timer = new Timer((state) => LoadDocuments(), null, 3000, 3000);
        }
        private void DownloadDocumentById(int? id)
        {
            if (!id.HasValue) return;

            var documentInfo = GetDocumentById(id.Value);
            if (documentInfo == null) return;
            if (documentInfo.IsDownloading) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = documentInfo.Title + Path.GetExtension(documentInfo.FileUri);
            saveFileDialog.RestoreDirectory = true;
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                documentInfo.DeleteBtnIsEnabled = false;
                documentInfo.IsDownloading = true;
                documentInfo.DownloadBtnContent = "下载中";

                Task.Run(() =>
                {
                    try
                    {
                        WebRequest request = WebRequest.Create(documentInfo.FileUri);
                        WebResponse respone = request.GetResponse();
                        long fileSize = respone.ContentLength;
                        using (Stream netStream = respone.GetResponseStream())
                        {
                            Stream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                            byte[] read = new byte[1024];
                            long progressBarValue = 0;
                            int realReadLen = netStream.Read(read, 0, read.Length);
                            while (realReadLen > 0)
                            {
                                fileStream.Write(read, 0, realReadLen);
                                progressBarValue += realReadLen;
                                documentInfo.DownloadBtnContent = "下载中(" + Math.Floor(progressBarValue / (double)fileSize * 100) + "%)";
                                realReadLen = netStream.Read(read, 0, read.Length);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("下载失败:" + ex.Message, "系统提示");
                    }
                    finally
                    {
                        documentInfo.DownloadBtnContent = "下载";
                        documentInfo.DownloadProgressValue = 0;
                        documentInfo.IsDownloading = false;
                        documentInfo.DeleteBtnIsEnabled = true;
                    }
                });


            }
        }
        private void DeleteDocumentById(int? id)
        {

            if (!id.HasValue) return;

            var documentInfo = GetDocumentById(id.Value);
            if (documentInfo == null) return;
            if (documentInfo.IsDeleting) return;

            documentInfo.IsDeleting = true;
            documentInfo.DownloadBtnIsEnabled = false;

            Task.Run(() =>
            {
                try
                {
                    var result = _documentService.DeleteDocument(documentInfo.Id);
                    if (!result.Item1)
                    {
                        MessageBox.Show(result.Item2, "系统提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "系统提示");
                    documentInfo.IsDeleting = false;
                    documentInfo.DownloadBtnIsEnabled = true;
                }
            });

        }
        private DocumentItemViewModel GetDocumentById(int id)
        {
            return Documents.Where(item => item.Id == id).FirstOrDefault();
        }
        private void UploadDocument()
        {
            OpenSelectFileDialog(_fileFilter, (fileFullName) =>
            {
                try
                {
                    Task.Run(() =>
                    {
                        var result = _documentService.UploadDocument(fileFullName, StreamTransferProgress);
                        if (!result.Item1)
                        {
                            MessageBox.Show(result.Item2, "系统提示");
                        }
                        IsUploadCompleted = true;
                        _isUploading = false;
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "系统提示");
                    _isUploading = false;
                    _logger.Error(ex.Message);
                }
            });
        }
        private void LoadDocuments()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            var documentList = _documentService.GetDocuments();
            try
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var item in documentList)
                    {
                        if (!Documents.Any(d => d.Id == item.ID))
                        {
                            Documents.Add(new DocumentItemViewModel()
                            {
                                Title = item.Title,
                                FileUri = item.FileUri,
                                Id = item.ID
                            });
                        }
                    }
                    for (int i = 0; i < Documents.Count; i++)
                    {
                        if (!documentList.Any(d => d.ID == Documents[i].Id))
                        {
                            Documents.Remove(Documents[i]);
                        }
                    }
                });
            }
            catch { }
            finally
            {
                _timer.Change(3000, 3000);
            }

        }
        private void StreamTransferProgress(object sender, StreamTransferProgressArgs args)
        {
            UplodingProgressValue = args.PercentDone;
        }
        private void OpenSelectFileDialog(string fileFilter, Action<string> callback)
        {
            if (IsUploadCompleted)
            {
                IsUploadCompleted = false;
                return;
            }
            if (_isUploading) return;
            var selectedFileName = OpenFileDialogHelper.OpenFileDialogWindow(fileFilter);
            if (string.IsNullOrEmpty(selectedFileName)) return;
            _isUploading = true;
            IsUploadCompleted = false;
            callback?.Invoke(selectedFileName);
        }
    }
}
