using PowerCreatorDotCom.Sdk.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PowerCreatorDotCom.Sdk.Core.Http
{
    public class HttpResponse : HttpRequest
    {
        private static int _timeout = 100000; // No effect
        private static int bufferLength = 1024;

        public int Status { get;  set; }
        public Exception Error { get;  set; }
        public HttpResponse() { }
        public HttpResponse(string strUrl)
            : base(strUrl) { }

        private static void PasrseHttpResponse(HttpResponse httpResponse, HttpWebResponse httpWebResponse)
        {
            httpResponse.Content = ReadContent(httpResponse, httpWebResponse);
            httpResponse.Status = (int)httpWebResponse.StatusCode;
            httpResponse.Headers = new Dictionary<string, string>();
            httpResponse.Method = ParameterHelper.StringToMethodType(httpWebResponse.Method);

            foreach (var key in httpWebResponse.Headers.AllKeys)
            {
                httpResponse.Headers.Add(key, httpWebResponse.Headers[key]);
            }

            string type = httpResponse.Headers["Content-Type"];
            if (null != type)
            {
                httpResponse.Encoding = "UTF-8";
                string[] split = type.Split(';');
                httpResponse.ContentType = ParameterHelper.StingToFormatType(split[0].Trim());
                if (split.Length > 1 && split[1].Contains("="))
                {
                    string[] codings = split[1].Split('=');
                    httpResponse.Encoding = codings[1].Trim().ToUpper();
                }
            }
        }

        public static byte[] ReadContent(HttpResponse response, HttpWebResponse rsp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferLength];
                Stream stream = rsp.GetResponseStream();

                while (true)
                {
                    int length = stream.Read(buffer, 0, bufferLength);
                    if (length == 0)
                    {
                        break;
                    }
                    ms.Write(buffer, 0, length);
                }
                ms.Seek(0, SeekOrigin.Begin);
                byte[] bytes = new byte[ms.Length];
                ms.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        public static HttpResponse GetResponse(HttpRequest request, int? timeout = null)
        {
            HttpWebRequest httpWebRequest = GetWebRequest(request);
            if (timeout != null)
            {
                httpWebRequest.Timeout = (int)timeout;
            }

            HttpResponse httpResponse = new HttpResponse(httpWebRequest.RequestUri.AbsoluteUri);
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    httpWebResponse = (HttpWebResponse)ex.Response;
                }
                else
                {
                    throw ex;
                }
            }

            PasrseHttpResponse(httpResponse, httpWebResponse);
            return httpResponse;
        }

        public static HttpWebRequest GetWebRequest(HttpRequest request)
        {
            HttpWebRequest httpWebRequest = null;
            httpWebRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            httpWebRequest.Method = request.Method.ToString();
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Timeout = _timeout;

            if (request.Headers.ContainsKey("Accept"))
            {
                httpWebRequest.Accept = DictionaryUtil.Pop(request.Headers, "Accept");
            }
            if (request.Headers.ContainsKey("Date"))
            {
                httpWebRequest.Date = Convert.ToDateTime(DictionaryUtil.Pop(request.Headers, "Date"));
            }

            foreach (var header in request.Headers)
            {
                if (header.Key.Equals("Content-Length"))
                {
                    httpWebRequest.ContentLength = long.Parse(header.Value);
                    continue;
                }
                if (header.Key.Equals("Content-Type"))
                {
                    httpWebRequest.ContentType = header.Value;
                    continue;
                }

                httpWebRequest.Headers.Add(header.Key, header.Value);
            }

            if ((request.Method == MethodType.POST || request.Method == MethodType.PUT) && request.Content != null)
            {
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(request.Content, 0, request.Content.Length);
                }
            }


            return httpWebRequest;
        }

        public bool IsSuccess
        {
            get
            {
                return 200 <= this.Status && 300 > this.Status;
            }
        }
    }
}
