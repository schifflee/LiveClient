using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerCreatorDotCom.Sdk.Core.Http;

namespace PowerCreatorDotCom.Sdk.Core
{
    public class ServiceClient : IServiceClient
    {
        private int maxRetryNumber = 3;
        private bool autoRetry = true;
        public HttpResponse DoAction<T>(ServiceRequest<T> request) where T : ServiceResponse
        {
            throw new NotImplementedException();
        }

        public HttpResponse DoAction<T>(ServiceRequest<T> request, bool autoRetry, int maxRetryCounts) where T : ServiceResponse
        {
            throw new NotImplementedException();
        }

        public T GetResponse<T>(ServiceRequest<T> request) where T : ServiceResponse
        {
            throw new NotImplementedException();
        }

        public T GetResponse<T>(ServiceRequest<T> request, bool autoRetry, int maxRetryCounts) where T : ServiceResponse
        {
            throw new NotImplementedException();
        }
    }
}
