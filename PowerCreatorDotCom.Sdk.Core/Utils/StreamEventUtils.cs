using PowerCreatorDotCom.Sdk.Core.Common;
using PowerCreatorDotCom.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreatorDotCom.Sdk.Core.Utils
{
    public static class StreamEventUtils
    {
        internal static Stream SetupProgressListeners(Stream originalStream, 
                                                      long contentLength,
                                                      long totalBytesRead,
                                                      long progressUpdateInterval,
                                                      object sender,
                                                      EventHandler<StreamTransferProgressArgs> callback)
        {
            var eventStream = new EventStream(originalStream, true);
            var tracker = new StreamReadTracker(sender, callback, contentLength, totalBytesRead, progressUpdateInterval);
            eventStream.OnRead += tracker.ReadProgress;
            return eventStream;
        }

        internal static void InvokeInBackground<T>(EventHandler<T> handler, T args, object sender) where T : EventArgs
        {
            if (handler == null) return;

            var list = handler.GetInvocationList();
            foreach (var call in list)
            {
                ((EventHandler<T>)call)?.Invoke(sender, args);
            }
        }
    }
    
}
