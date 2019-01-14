using PowerCreatorDotCom.Sdk.Core.Models;
using System;

namespace PowerCreatorDotCom.Sdk.Core.Common
{
    internal class StreamReadTracker : StreamTransferTracker
    {
        internal StreamReadTracker(object sender, EventHandler<StreamTransferProgressArgs> callback,
                                   long contentLength, long totalBytesRead, long progressUpdateInterval)
            : base(sender, callback, contentLength, totalBytesRead, progressUpdateInterval)
        {
        }

        public void ReadProgress(int bytesRead)
        {
            TransferredProgress(bytesRead);
        }
    }
}
