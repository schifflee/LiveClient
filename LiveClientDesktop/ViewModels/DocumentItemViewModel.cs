using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using System;

namespace LiveClientDesktop.ViewModels
{
    public class DocumentItemViewModel : NotificationObject
    {
        public DocumentItemViewModel()
        {
            DeleteBtnContent = "删除";
            DownloadBtnContent = "下载";
            DeleteBtnIsEnabled = true;
            DownloadBtnIsEnabled = true;
        }
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                this.RaisePropertyChanged("Title");
            }
        }

        private string fileUri;

        public string FileUri
        {
            get { return fileUri; }
            set
            {
                fileUri = value;
                this.RaisePropertyChanged("FileUri");
            }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged("Id");
            }
        }

        private double downloadProgressValue;

        public double DownloadProgressValue
        {
            get { return downloadProgressValue; }
            set
            {
                downloadProgressValue = value;
                this.RaisePropertyChanged("DownloadProgressValue");
            }
        }

        private bool isDownloading;

        public bool IsDownloading
        {
            get { return isDownloading; }
            set
            {
                isDownloading = value;
                this.RaisePropertyChanged("IsDownloading");
            }
        }

        private string downloadBtnContent;

        public string DownloadBtnContent
        {
            get { return downloadBtnContent; }
            set
            {
                downloadBtnContent = value;
                this.RaisePropertyChanged("DownloadBtnContent");
            }
        }

        private string deleteBtnShowContent;

        public string DeleteBtnContent
        {
            get { return deleteBtnShowContent; }
            set
            {
                deleteBtnShowContent = value;
                this.RaisePropertyChanged("DeleteBtnShowContent");
            }
        }

        private bool isDeleteng;

        public bool IsDeleting
        {
            get { return isDeleteng; }
            set
            {
                isDeleteng = value;
                this.RaisePropertyChanged("IsDeleteng");
            }
        }

        private bool deleteBtnIsEnabled;

        public bool DeleteBtnIsEnabled
        {
            get { return deleteBtnIsEnabled; }
            set
            {
                deleteBtnIsEnabled = value;
                this.RaisePropertyChanged("DeleteBtnIsEnabled");
            }
        }
        private bool downloadBtnIsEnabled;

        public bool DownloadBtnIsEnabled
        {
            get { return downloadBtnIsEnabled; }
            set
            {
                downloadBtnIsEnabled = value;
                this.RaisePropertyChanged("DownloadBtnIsEnabled");
            }
        }
        public override string ToString()
        {
            return $"{Title},{Id}";
        }


    }
}
