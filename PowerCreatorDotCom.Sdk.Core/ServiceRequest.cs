using Newtonsoft.Json;
using PowerCreatorDotCom.Sdk.Core.Http;
using PowerCreatorDotCom.Sdk.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerCreatorDotCom.Sdk.Core
{
    public abstract class ServiceRequest<T> : HttpRequest
    {
        private ProtocolType protocol = ProtocolType.HTTP;
        private FormatType acceptFormat;

        public String Domian { get; }
        public String Version { get; }
        public String Controller { get; }
        public String ActionName { get; }

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

        public Dictionary<String, IEnumerable<string>> QueryParameters { get; private set; }

        public Dictionary<String, String> DomainParameters { get; private set; }

        public Dictionary<String, String> BodyParameters { get; private set; }
        public ServiceRequest(String domain)
            : base(null)
        {
            DictionaryUtil.Add(Headers, "x-sdk-client", "Net/1.0.0");
            DictionaryUtil.Add(Headers, "x-sdk-invoke-type", "normal");
            Domian = domain;
            Initialize();
        }
        public ServiceRequest(String domain, String version)
            : this(domain)
        {
            Version = version;
        }
        public ServiceRequest(String domain, String version, String action)
           : this(domain, version)
        {
            ActionName = action;
        }
        public ServiceRequest(String domain, String version, String controller, String action)
            : this(domain, version, action)
        {
            Controller = controller;
        }
        private void Initialize()
        {
            Method = MethodType.GET;
            AcceptFormat = FormatType.JSON;
        }

        private String ConcatQueryString(Dictionary<string, IEnumerable<string>> parameters)
        {
            if (null == parameters)
            {
                return null;
            }
            return parameters.Keys.Aggregate(string.Empty, (queryparamString, key) =>
            {
                if (!string.IsNullOrEmpty(queryparamString))
                {
                    queryparamString += "&";
                }
                if (parameters[key].Count() > 1)
                {
                    return $"{queryparamString}{parameters[key].Aggregate((p1, p2) => string.Format("{0}={1}&{0}={2}", key, p1.ToString(), p2.ToString()))}";
                }
                return $"{queryparamString}{key}={parameters[key].FirstOrDefault()}";
            });
        }
        protected virtual void AddQueryParameters(string key, params bool[] values)
        {
            AddQueryParameters(key, values.Select(item => item.ToString()).ToArray());
        }
        protected virtual void AddQueryParameters(string key, params int[] values)
        {
            AddQueryParameters(key, values.Select(item => item.ToString()).ToArray());
        }
        protected virtual void AddQueryParameters(string key, params string[] values)
        {
            if (null == values)
            {
                return;
            }
            if (QueryParameters == null)
            {
                QueryParameters = new Dictionary<string, IEnumerable<string>>();
            }
            if (QueryParameters.ContainsKey(key))
            {
                QueryParameters.Remove(key);
            }
            QueryParameters.Add(key, values);
        }
        public HttpRequest ConstructRequest()
        {
            StringBuilder urlBuilder = new StringBuilder("");
            if (!Domian.Contains("http"))
            {
                urlBuilder.Append(this.Protocol.ToString().ToLower());
                urlBuilder.Append("://");
                urlBuilder.Append(Domian);
            }
            else
            {
                urlBuilder.Append(Domian);
            }
            if (!string.IsNullOrEmpty(Version))
            {
                urlBuilder.Append("/" + Version);
            }
            urlBuilder.Append("/" + Controller);
            urlBuilder.Append("/" + ActionName);
            if (-1 == urlBuilder.ToString().IndexOf("?"))
            {
                urlBuilder.Append("?");
            }
            String query = ConcatQueryString(QueryParameters);
            Url = urlBuilder.Append(query).ToString();
            return this;
        }

        public virtual ServiceResponseResult<T> GetResponse(string responseContext, FormatType? format)
        {
            try
            {
                switch (format)
                {
                    case FormatType.JSON:
                        return JsonConvert.DeserializeObject<ServiceResponseResult<T>>(responseContext);
                    default:
                        return JsonConvert.DeserializeObject<ServiceResponseResult<T>>(responseContext);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponseResult<T>
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
