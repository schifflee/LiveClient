﻿using PowerCreatorDotCom.Sdk.Core.Models;
using PowerCreatorDotCom.Sdk.Core.Utils;
using System;

namespace PowerCreatorDotCom.Sdk.Core.Common
{
    internal class StreamTransferTracker
    {
        private object sender;
        private EventHandler<StreamTransferProgressArgs> callback;
        private long contentLength;
        private long totalBytesTransferred;
        private long totalIncrementTransferred;
        private long progressUpdateInterval;

        internal StreamTransferTracker(object sender, EventHandler<StreamTransferProgressArgs> callback,
                                   long contentLength, long totalBytesTransferred, long progressUpdateInterval)
        {
            this.sender = sender;
            this.callback = callback;
            this.contentLength = contentLength;
            this.totalBytesTransferred = totalBytesTransferred;
            this.progressUpdateInterval = progressUpdateInterval;
        }

        public void TransferredProgress(int bytesTransferred)
        {
            if (callback == null)
                return;

            // Invoke the progress callback only if bytes read > 0
            if (bytesTransferred > 0)
            {
                totalBytesTransferred += bytesTransferred;
                totalIncrementTransferred += bytesTransferred;

                if (totalIncrementTransferred >= this.progressUpdateInterval ||
                    totalBytesTransferred == contentLength)
                {
                    StreamEventUtils.InvokeInBackground(
                                        callback,
                                        new StreamTransferProgressArgs(totalIncrementTransferred, totalBytesTransferred, contentLength),
                                        sender);
                    totalIncrementTransferred = 0;
                }
            }
        }

        public void UpdateProgress(float progress)
        {
            int bytesTransferred = (int)((long)(progress * contentLength) - totalBytesTransferred);
            TransferredProgress(bytesTransferred);
        }
    }
}
