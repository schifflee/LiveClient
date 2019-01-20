using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Xml;

namespace LiveClientLancher
{
   
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        XmlDocument receivedAppCastDocument = new XmlDocument();
        XmlDocument localAppVersionDocument = new XmlDocument();
        private string appDir;
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            if (e.Args.Length <= 0)
            {
                MessageBox.Show("缺少客户端启动参数", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
                return;
            }
            appDir = AppDomain.CurrentDomain.BaseDirectory;
            string startupParamsStr = e.Args[0];//"powercreator://160|d38e32747b9e4e5d9868e801941af7e6|http://miyun.smartclass.cn";

            try
            {
                localAppVersionDocument.Load(appDir + "version.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取本地配置文件失败:" + ex.Message, "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
                return;
            }

            XmlNode localVersionXmlRootNode = localAppVersionDocument.SelectSingleNode("root");
            int.TryParse(localVersionXmlRootNode?.SelectSingleNode("version")?.InnerText, out int currentVersion);

            string args = HttpUtility.UrlDecode(startupParamsStr).Replace("powercreator://", "");
            var argsArr = args.Split('|');
            string appCastUrl = argsArr[2];

            var webRequest = WebRequest.Create(appCastUrl + "/LiveClient/versionlist.xml?m=" + DateTime.Now.Ticks);
            webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            WebResponse webResponse = null;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch (Exception ex)
            {
                MessageBox.Show("远程服务器连接失败:" + ex.Message, "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
                return;
            }
            List<VersionInfo> versionList = new List<VersionInfo>();
            using (Stream appCastStream = webResponse.GetResponseStream())
            {
                if (appCastStream != null)
                {
                    try
                    {
                        receivedAppCastDocument.Load(appCastStream);
                        XmlNodeList appCastItems = receivedAppCastDocument.GetElementsByTagName("versionitem");
                        if (appCastItems != null)
                        {
                            foreach (XmlNode item in appCastItems)
                            {
                                int.TryParse(item.SelectSingleNode("version").InnerText, out int versionNum);
                                if (versionNum <= currentVersion)
                                {
                                    continue;
                                }
                                VersionInfo versionInfo = new VersionInfo();
                                versionInfo.Version = versionNum;
                                versionInfo.VersionName = item.SelectSingleNode("versionname").InnerText;
                                versionInfo.DownloadUrl = item.SelectSingleNode("downloadurl").InnerText;
                                versionInfo.ChangeLog = item.SelectSingleNode("changelog").InnerText;
                                versionList.Add(versionInfo);
                            }
                        }
                        else
                        {
                            webResponse.Close();
                        }
                    }
                    catch (XmlException xmlex)
                    {
                        webResponse.Close();
                    }

                }
            }

            if (!versionList.Any())
            {
                Process miyunClientProcess = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(appDir + "LiveClientDesktop.exe", startupParamsStr);
                miyunClientProcess.StartInfo = startInfo;
                miyunClientProcess.StartInfo.UseShellExecute = false;
                miyunClientProcess.Start();
                App.Current.Shutdown();
                return;
            }

            StartupParams.Instance.AppCastUrl = appCastUrl;
            StartupParams.Instance.ClientStartupParams = startupParamsStr;
            StartupParams.Instance.CurrentVersion = currentVersion;
            StartupParams.Instance.LastVersionList = versionList;
            StartupParams.Instance.CurrentVersionName = localVersionXmlRootNode?.SelectSingleNode("versionname")?.InnerText;
            StartupParams.Instance.appDir = appDir;

        }
    }
}
