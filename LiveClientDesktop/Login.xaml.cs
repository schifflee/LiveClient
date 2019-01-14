using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Unity;
using PowerCreator.WebPlatform.Sdk;
using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;
using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LiveClientDesktop
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private readonly AutoLoginService _autoLoginService;
        private readonly LiveInfo _liveInfo;
        private readonly IUnityContainer _container;
        public Login(IUnityContainer container)
        {
            InitializeComponent();
            _autoLoginService = container.Resolve<AutoLoginService>();
            _liveInfo = container.Resolve<LiveInfo>();
            _container = container;

            

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Thread.Sleep(1000);

                var loginResult = _autoLoginService.Login();
                if (!loginResult.Item1) ShowMsgAndCloseThisWindow(loginResult.Item2);

                var getLiveInfoResult = _autoLoginService.GetLiveInfo();
                if (!getLiveInfoResult.Success) ShowMsgAndCloseThisWindow(getLiveInfoResult.Message);

                SetLiveInfo(getLiveInfoResult.Value);

                //var service = _container.Resolve<IServiceClient>();
                //var start = _container.Resolve<StartupParameters>();
                //using (var fs = File.Open(@"E:\密云直播课堂方案0613.pptx", FileMode.Open))
                //{
                //    UploadDocument uploadDocument = new UploadDocument("47.93.38.164", "613", "密云直播课堂方案0613", "张老师", ".pptx", fs);
                //    uploadDocument.UseChunkedEncoding = true;
                //    uploadDocument.StreamTransferProgress += streamProgressCallback;
                //    uploadDocument.Headers.Add("Cookie", start.UserIdentity);
                //    var s = service.GetResponse(uploadDocument);
                //}


                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        Shell shell = _container.Resolve<Shell>();
                        Application.Current.MainWindow = shell;
                        this.Close();
                        shell.Show();
                    }
                    catch (Exception ex)
                    {
                        ShowMsgAndCloseThisWindow(ex.Message);
                    }
                });
            });

        }
        private void SetLiveInfo(GetLiveInfoResponse getLiveInfoResponse)
        {
            _liveInfo.LiveID = getLiveInfoResponse.LiveID;
            _liveInfo.ScheduleID = getLiveInfoResponse.ScheduleID;
            _liveInfo.StartTime = getLiveInfoResponse.StartTime;
            _liveInfo.StopTime = getLiveInfoResponse.StopTime;
            _liveInfo.TeacherList = getLiveInfoResponse.TeacherList;
            _liveInfo.Title = HttpUtility.HtmlDecode(getLiveInfoResponse.Title);
        }
        private void ShowMsgAndCloseThisWindow(string msg)
        {
            Dispatcher.Invoke(() =>
            {
                Close();
                MessageBox.Show(msg, "系统提示");
            });
        }
    }
}
