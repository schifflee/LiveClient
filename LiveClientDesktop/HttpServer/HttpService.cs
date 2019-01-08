using LiveClientDesktop.HttpRequestHandler;
using Microsoft.Practices.Prism.Logging;
using System.Collections.Generic;
using System.IO;

namespace LiveClientDesktop.HttpServer
{
    internal class HttpService : HttpServer
    {
        public HttpService(int port)
            : base(port)
        {
        }
        public override void handleGETRequest(HttpProcessor p)
        {
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            string actionName = string.Empty;
            if (p.http_url.IndexOf("?") > -1)
            {
                actionName = p.http_url.Substring(1, p.http_url.IndexOf("?") - 1);
            }
            else
            {
                actionName = p.http_url.Substring(1);
            }

            if (p.http_url.IndexOf("?") > -1)
            {
                string strParams = p.http_url.Substring(p.http_url.IndexOf("?") + 1);
                foreach (var item in strParams.Split('&'))
                {
                    string paramName = string.Empty,
                           paramValue = string.Empty;
                    var param = item.Split('=');
                    if (param.Length > 1)
                    {
                        paramName = param[0];
                        paramValue = param[1];
                    }
                    else if (param.Length == 1)
                    {
                        paramName = param[0];
                    }
                    if (!urlParams.ContainsKey(paramName) && !string.IsNullOrEmpty(paramName))
                    {
                        urlParams.Add(paramName, paramValue);
                    }
                }
            }
            if (HttpRequestHandlerManager.Instance.HandlerList.ContainsKey(actionName))
            {
                p.httpHeaders.Add("Copyright", "PowerCreator");
                p.httpHeaders.Add("Access-Control-Allow-Origin", "*");
                p.writeSuccess();
                p.outputStream.WriteLine(HttpRequestHandlerManager.Instance.HandlerList[actionName](urlParams));
            }
            else
            {
                p.writeFailure();
            }
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            p.outputStream.WriteLine("1");
        }
    }
}
