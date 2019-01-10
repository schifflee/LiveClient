using PowerCreatorDotCom.Sdk.Core.Http;
using PowerCreatorDotCom.Sdk.Core.Transform;
using PowerCreatorDotCom.Sdk.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreatorDotCom.Sdk.Core
{
    public abstract class ServiceRequest<T> : HttpRequest
    {
        private ProtocolType protocol = ProtocolType.HTTP;
        private FormatType acceptFormat;

        public virtual String Product { get; set; }
        public virtual String Version { get; set; }
        public virtual String ActionName { get; set; }

        public virtual FormatType AcceptFormat
        {
            get
            {
                return acceptFormat;
            }
            set
            {
                acceptFormat = value;
                DictionaryUtil.Add(Headers, "Accept", value.ToString());
            }
        }
        public ProtocolType Protocol
        {
            get
            {
                return protocol;
            }
            set
            {
                protocol = value;
            }
        }

        public Dictionary<String, String> QueryParameters
        {
            get; set;
        }

        public Dictionary<String, String> DomainParameters
        {
            get; set;
        }

        public Dictionary<String, String> BodyParameters
        {
            get; set;
        }
        public ServiceRequest(String product)
           : base(null)
        {
            DictionaryUtil.Add(Headers, "x-sdk-client", "Net/1.0.0");
            DictionaryUtil.Add(Headers, "x-sdk-invoke-type", "normal");
            Product = product;
        }
        public ServiceRequest(String product, String version)
            : this(product)
        {
            Version = version;
            Initialize();
        }

        public ServiceRequest(String product, String version, String action)
            : this(product)
        {
            Version = version;
            ActionName = action;
            Initialize();
        }
        private void Initialize()
        {
            Method = MethodType.GET;
            AcceptFormat = FormatType.JSON;
        }

        private String ConcatQueryString(Dictionary<String, String> parameters)
        {
            if (null == parameters)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();

            foreach (var entry in parameters)
            {
                String key = entry.Key;
                String val = entry.Value;

                sb.Append(URLEncoder.Encode(key));
                if (val != null)
                {
                    sb.Append("=").Append(URLEncoder.Encode(val));
                }
                sb.Append("&");
            }

            int strIndex = sb.Length;
            if (parameters.Count > 0)
                sb.Remove(strIndex - 1, 1);

            return sb.ToString();
        }
        public HttpRequest ConstructRequest()
        {
            Dictionary<String, String> mapQueries = this.QueryParameters;
            StringBuilder urlBuilder = new StringBuilder("");
            urlBuilder.Append(this.Protocol.ToString().ToLower());
            urlBuilder.Append("://").Append(Url);
            urlBuilder.Append("/" + Version);
            urlBuilder.Append("/" + ActionName);
            if (-1 == urlBuilder.ToString().IndexOf("?"))
            {
                urlBuilder.Append("/?");
            }
            String query = ConcatQueryString(mapQueries);
            Url = urlBuilder.Append(query).ToString();
            return this;
        }

        public abstract T GetResponse(UnmarshallerContext unmarshallerContext);
    }
}
