using PowerCreatorDotCom.Sdk.Core.Utils;
using System;
using System.Collections.Generic;

namespace PowerCreatorDotCom.Sdk.Core.Http
{
    public class HttpRequest
    {
        public Dictionary<string, string> Headers { get; set; }
        public String Url { get; set; }
        public MethodType? Method { get; set; }
        public FormatType? ContentType { get; set; }
        public byte[] Content { get; set; }
        public String Encoding { get; set; }

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

        public void SetContent(byte[] content, String encoding, FormatType? format)
        {
            if (null == content)
            {
                Headers.Remove("Content-Length");
                Headers.Remove("Content-Type");
                ContentType = null;
                Content = null;
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

            this.Headers.Remove("Content-Length");
            this.Headers.Remove("Content-Type");
            this.Headers.Add("Content-Length", contentLen);
            this.Headers.Add("Content-Type", ParameterHelper.FormatTypeToString(type));

            this.Content = content;
            this.Encoding = encoding;
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
