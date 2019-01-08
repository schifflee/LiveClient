using LiveClientDesktop.HttpRequestHandler;
using System;
using System.Collections.Generic;

namespace LiveClientDesktop.ViewModels
{
    public partial class LiveControlViewModel
    {
        public LiveControlViewModel()
        {
            HttpRequestHandlerManager.Instance.AddHandler("StartLive", new Func<IDictionary<string, string>, string>((dic) =>
            {
                string msg = string.Empty;
                Tuple<bool, string> result = _StartLive();
                return HttpRequestHandleResultWarpper.WriteResult(result.Item1, result.Item2);
            }));
            HttpRequestHandlerManager.Instance.AddHandler("PauseLive", new Func<IDictionary<string, string>, string>((dic) =>
            {
                string msg = string.Empty;
                Tuple<bool, string> result = _PauseLive();
                return HttpRequestHandleResultWarpper.WriteResult(result.Item1, result.Item2);
            }));
            HttpRequestHandlerManager.Instance.AddHandler("StopLive", new Func<IDictionary<string, string>, string>((dic) =>
            {
                string msg = string.Empty;
                Tuple<bool, string> result = _StopLive();
                return HttpRequestHandleResultWarpper.WriteResult(result.Item1, result.Item2);
            }));
        }
    }
}
