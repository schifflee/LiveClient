using PowerCreatorDotCom.Sdk.Core.Http;

namespace PowerCreatorDotCom.Sdk.Core
{
    public interface IServiceClient
    {
        ServiceResponseResult<T> GetResponse<T>(ServiceRequest<T> request) where T : ServiceResponse;

        ServiceResponseResult<T> GetResponse<T>(ServiceRequest<T> request, bool autoRetry, int maxRetryCounts) where T : ServiceResponse;

        HttpResponse DoAction<T>(ServiceRequest<T> request) where T : ServiceResponse;
        HttpResponse DoAction<T>(ServiceRequest<T> request, bool autoRetry, int maxRetryCounts) where T : ServiceResponse;
    }
}
