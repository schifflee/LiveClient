using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml;

namespace LiveClientLancher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DownloadWebClient _webClient;
        private string _tempFile;
        private string _currentInstallVersionName;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentVersion.Content = StartupParams.Instance.CurrentVersionName;
            LastCurrentVersion.Content = StartupParams.Instance.LastVersionList.Last().VersionName;
            Task.Run(async () =>
            {
                await StartUpdate();
            });
        }

        private async Task RunLiveClient()
        {
            await Task.Run(() =>
            {
                Process miyunClientProcess = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(StartupParams.Instance.appDir + "LiveClientDesktop.exe", StartupParams.Instance.ClientStartupParams);
                miyunClientProcess.StartInfo = startInfo;
                miyunClientProcess.StartInfo.UseShellExecute = false;
                miyunClientProcess.Start();
                UpdateControlState(() =>
                {
                    App.Current.Shutdown();
                });
            });

        }
        private async Task StartUpdate()
        {
            await StartDownloadAndInstall();
            WaitingStartupMiYunClient();
            await RunLiveClient();
        }
        private void WaitingStartupMiYunClient()
        {
            UpdateControlState(() =>
            {
                DownloadVersionText.Text = "客户端已升级至最新版本";
            });
            Thread.Sleep(1000);
            UpdateControlState(() =>
            {
                DownloadVersionText.Text = "正在启动密云直播云课堂";
            });
            Thread.Sleep(1000);
        }
        private async Task StartDownloadAndInstall()
        {
            foreach (var item in StartupParams.Instance.LastVersionList)
            {
                _currentInstallVersionName = item.VersionName;
                UpdateControlState(() =>
                {
                    ChangeLog.Text = item.ChangeLog;
                });
                UpdateControlState(() =>
                {
                    DownloadVersionText.Text = $"正在下载{_currentInstallVersionName}版本的更新包";
                });
                _webClient = new DownloadWebClient { CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore) };
                _tempFile = Path.GetTempFileName();
                var uri = new Uri(item.DownloadUrl);
                _webClient.DownloadProgressChanged += _webClient_DownloadProgressChanged; ;

                _webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                await _webClient.DownloadFileTaskAsync(uri, _tempFile);
                SaveUpdateVersionInfo(item.Version, item.VersionName);
            }
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            UpdateControlState(() =>
            {
                DownloadVersionText.Text = $"正在安装{_currentInstallVersionName}版本的更新包";
            });
            string fileName;
            string contentDisposition = _webClient.ResponseHeaders["Content-Disposition"] ?? string.Empty;
            if (string.IsNullOrEmpty(contentDisposition))
            {
                fileName = Path.GetFileName(_webClient.ResponseUri.LocalPath);
            }
            else
            {
                fileName = _tryToFindFileName(contentDisposition, "filename=");
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = _tryToFindFileName(contentDisposition, "filename*=UTF-8''");
                }
            }
            var tempPath = Path.Combine(Path.GetTempPath(), fileName);

            try
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                File.Move(_tempFile, tempPath);
            }
            catch (Exception ex)
            {
                UpdateControlState(() =>
                {
                    DownloadVersionText.Text = $"安装{_currentInstallVersionName}版本的更新包失败," + ex.Message;
                });
                _webClient = null;
                return;
            }
            var path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            ZipStorer zip = ZipStorer.Open(tempPath, FileAccess.Read);

            List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();

            for (var index = 0; index < dir.Count; index++)
            {
                ZipStorer.ZipFileEntry entry = dir[index];
                zip.ExtractFile(entry, Path.Combine(path, entry.FilenameInZip));
                int progress = (index + 1) * 100 / dir.Count; ;
                UpdateControlState(() =>
                {
                    DownloadProgressBar.Value = progress;
                });
                UpdateControlState(() =>
                {
                    DownloadProgressText.Text = progress + "%";
                });
            }
            zip.Close();

            UpdateControlState(() =>
            {
                DownloadVersionText.Text = $"版本{_currentInstallVersionName}安装成功";
            });
            UpdateControlState(() =>
            {
                DownloadProgressText.Text = string.Empty;
            });
            UpdateControlState(() =>
            {
                CurrentVersion.Content = _currentInstallVersionName;
            });
        }

        private void SaveUpdateVersionInfo(int versionNum, string versionName)
        {
            XmlDocument localAppVersionDocument = new XmlDocument();
            localAppVersionDocument.Load(StartupParams.Instance.appDir + "version.xml");
            XmlNode rootNode = localAppVersionDocument.SelectSingleNode("root");
            rootNode.SelectSingleNode("version").InnerText = versionNum.ToString();
            rootNode.SelectSingleNode("versionname").InnerText = versionName;
            localAppVersionDocument.Save(StartupParams.Instance.appDir + "version.xml");
        }
        private void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            UpdateControlState(() =>
            {
                DownloadProgressBar.Value = e.ProgressPercentage;
            });
            UpdateControlState(() =>
            {
                DownloadProgressText.Text = $"{e.ProgressPercentage}%";
            });
        }

        private void UpdateControlState(Action action)
        {
            this.Dispatcher.Invoke(action);
        }
        private string _tryToFindFileName(string contentDisposition, string lookForFileName)
        {
            string fileName = String.Empty;
            if (!string.IsNullOrEmpty(contentDisposition))
            {
                var index = contentDisposition.IndexOf(lookForFileName, StringComparison.CurrentCultureIgnoreCase);
                if (index >= 0)
                    fileName = contentDisposition.Substring(index + lookForFileName.Length);
                if (fileName.StartsWith("\""))
                {
                    var file = fileName.Substring(1, fileName.Length - 1);
                    var i = file.IndexOf("\"", StringComparison.CurrentCultureIgnoreCase);
                    if (i != -1)
                    {
                        fileName = file.Substring(0, i);
                    }
                }
            }
            return fileName;
        }
    }
    public class DownloadWebClient : WebClient
    {
        public Uri ResponseUri;

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse webResponse = base.GetWebResponse(request, result);
            ResponseUri = webResponse.ResponseUri;
            return webResponse;
        }
    }
}
