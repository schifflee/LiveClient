using PowerCreatorDotCom.Sdk.Core.Models;
using PowerCreatorDotCom.Sdk.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace PowerCreatorDotCom.Sdk.Core.Http
{
    public class HttpRequest
    {
        public Dictionary<string, string> Headers { get; set; }
        public String Url { get; set; }
        public MethodType? Method { get; set; }
        public FormatType? ContentType { get; set; }
        public Stream BodyContent { get; set; }
        public String Encoding { get; set; }

        public EventHandler<StreamTransferProgressArgs> StreamTransferProgress { get; set; }
        public bool UseChunkedEncoding { get; set; }

        private int timeoutInMilliSeconds = 100000;

        public HttpRequest() { }
        public HttpRequest(String url)
        {
            Url = url;
            Headers = new Dictionary<string, string>();
        }

        public HttpRequest(String url, Dictionary<string, string> temHeaders)
        {
            Url = url;
            if (null != temHeaders) Headers = temHeaders;
        }

        public void SetContent(Stream content, FormatType? format)
        {
            if (null == content)
            {
                Headers.Remove("Content-Length");
                Headers.Remove("Content-Type");
                ContentType = null;
                BodyContent = null;
                Encoding = null;
                return;
            }
            String contentLen = content.Length.ToString();
            FormatType? type = FormatType.RAW;
            if (null != format)
            {
                ContentType = format;
                type = format;
            }

            if (Method == MethodType.POST || Method == MethodType.PUT)
            {
                this.Headers.Remove("Content-Length");
                this.Headers.Remove("Content-Type");
                this.Headers.Add("Content-Length", contentLen);
                this.Headers.Add("Content-Type", ParameterHelper.FormatTypeToString(type));
            }

            this.BodyContent = content;
        }

        public int TimeoutInMilliSeconds
        {
            get
            {
                return timeoutInMilliSeconds;
            }
            set
            {
                timeoutInMilliSeconds = value;
            }
        }
    }
}
