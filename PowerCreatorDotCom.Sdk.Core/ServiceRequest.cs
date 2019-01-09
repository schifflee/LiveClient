using PowerCreatorDotCom.Sdk.Core.Http;
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
        private Dictionary<String, String> queryParameters = new Dictionary<String, String>();
        private Dictionary<String, String> domainParameters = new Dictionary<String, String>();
        private Dictionary<String, String> bodyParameters = new Dictionary<String, String>();

    }
}
