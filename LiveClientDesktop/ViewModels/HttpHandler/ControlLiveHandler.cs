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
                return Processor(_StartLive);
            }));
            HttpRequestHandlerManager.Instance.AddHandler("PauseLive", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return Processor(_PauseLive);
            }));
            HttpRequestHandlerManager.Instance.AddHandler("StopLive", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return Processor(_StopLive);
            }));
        }
        private string Processor(Func<Tuple<bool,string>> handler)
        {
            Tuple<bool, string> result = handler.Invoke();
            return HttpRequestHandleResultWarpper.WriteResult(result.Item1, result.Item2);
        }
    }
}
