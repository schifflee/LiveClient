﻿using System;
using System.Collections.Generic;

namespace LiveClientDesktop.HttpRequestHandler
{
    public class HttpRequestHandlerManager
    {
        public static HttpRequestHandlerManager Instance { get; } = new HttpRequestHandlerManager();
        public IDictionary<String, Func<IDictionary<String, String>, String>> HandlerList { get; private set; }
        private HttpRequestHandlerManager()
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