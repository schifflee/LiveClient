using PowerCreatorDotCom.Sdk.Core.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreatorDotCom.Sdk.Core
{
    public interface IServiceClient
    {
        T GetResponse<T>(ServiceRequest<T> request) where T : ServiceResponse;

        T GetResponse<T>(ServiceRequest<T> request, bool autoRetry, int maxRetryCounts) where T : ServiceResponse;

        HttpResponse DoAction<T>(ServiceRequest<T> request) where T : ServiceResponse;
        HttpResponse DoAction<T>(ServiceRequest<T> request, bool autoRetry, int maxRetryCounts) where T : ServiceResponse;
    }
}
