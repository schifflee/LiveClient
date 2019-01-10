using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PowerCreatorDotCom.Sdk.Core
{
    public abstract class ServiceResponse
    {
        public HttpResponse HttpResponse { get; set; }
    }
}
