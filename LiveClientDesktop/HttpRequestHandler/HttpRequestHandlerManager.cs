using System;
using System.Collections.Generic;

namespace LiveClientDesktop.HttpRequestHandler
{
    public class HttpRequestHandleManager
    {
        //TODO 客户端使用不方便需要重构
        public static HttpRequestHandleManager Instance { get; } = new HttpRequestHandleManager();
        public IDictionary<String, Func<IDictionary<String, String>, String>> HandlerList { get; private set; }
        private HttpRequestHandleManager()
        {
            HandlerList = new Dictionary<String, Func<IDictionary<String, String>, String>>();
        }

        public void AddHandler(string handlerName, Func<IDictionary<string, string>, string> func)
        {
            if (!HandlerList.ContainsKey(handlerName))
            {
                HandlerList.Add(handlerName, func);
            }
        }


    }
}
