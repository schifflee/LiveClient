using Newtonsoft.Json;

namespace LiveClientDesktop.HttpRequestHandler
{
    public static class HttpRequestHandleResultWarpper
    {
        public static string WriteResult(bool success, string msg = "", object value = null)
        {
            return JsonConvert.SerializeObject(new { Success = success, Message = msg, Value = value });
        }
    }
}
