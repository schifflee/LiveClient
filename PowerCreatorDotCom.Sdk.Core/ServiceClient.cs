using System;
using System.Text;
using PowerCreatorDotCom.Sdk.Core.Exceptions;
using PowerCreatorDotCom.Sdk.Core.Http;

namespace PowerCreatorDotCom.Sdk.Core
{
    public class ServiceClient : IServiceClient
    {
        private int maxRetryNumber = 3;
        private bool autoRetry = true;
        public HttpResponse DoAction<T>(ServiceRequest<T> request) where T : ServiceResponse
        {
            return DoAction(request, autoRetry, maxRetryNumber);
        }
        public HttpResponse DoAction<T>(ServiceRequest<T> request, bool autoRetry, int maxRetryCounts) where T : ServiceResponse
        {
            bool shouldRetry = true;
            for (int retryTimes = 0; shouldRetry; retryTimes++)
            {
                shouldRetry = autoRetry && retryTimes < maxRetryNumber;
                HttpRequest httpRequest = request.BuildRequestUri();
                HttpResponse response=new HttpResponse();
                try
                {
                    response = response.GetResponse(httpRequest);
                    if (response.Content == null)
                    {
                        if (shouldRetry)
                        {
                            continue;
                        }
                        else
                        {
                            return new HttpResponse { Status = -1, Error = new ClientException("Connection reset.") };
                        }
                    }

                    if (500 <= response.Status && shouldRetry)
                    {
                        continue;
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    return new HttpResponse { Status = -1, Error = ex };
                }
            }
            return new HttpResponse { Status = -1 };
        }

        public ServiceResponseResult<T> GetResponse<T>(ServiceRequest<T> request) where T : ServiceResponse
        {
            HttpResponse httpResponse = DoAction(request);

            return ParseResponse(request, httpResponse);
        }




        public ServiceResponseResult<T> GetResponse<T>(ServiceRequest<T> request, bool autoRetry, int maxRetryCounts) where T : ServiceResponse
        {
            HttpResponse httpResponse = DoAction(request, autoRetry, maxRetryCounts);

            return ParseResponse(request, httpResponse);
        }

        

        private ServiceResponseResult<T> ParseResponse<T>(ServiceRequest<T> request, HttpResponse httpResponse) where T : ServiceResponse
        {
            FormatType? format = httpResponse.ContentType;

            if (httpResponse.IsSuccess)
            {
                return ReadResponse(request, httpResponse, format);
            }
            else
            {
                return ReadError(request, httpResponse, format);
            }
        }
        private ServiceResponseResult<T> ReadError<T>(ServiceRequest<T> request, HttpResponse httpResponse, FormatType? format) where T : ServiceResponse
        {

            return new ServiceResponseResult<T>
            {
                Success = false,
                Message = httpResponse.Error == null ? httpResponse.Status.ToString() : httpResponse.Error.Message,
                HttpResponse = httpResponse

            };
        }
        private ServiceResponseResult<T> ReadResponse<T>(ServiceRequest<T> request, HttpResponse httpResponse, FormatType? format) where T : ServiceResponse
        {
            string body = Encoding.UTF8.GetString(httpResponse.Content);
            var rsp = request.GetResponse(body, httpResponse.ContentType);
            rsp.HttpResponse = httpResponse;
            return rsp;
        }
    }
}
