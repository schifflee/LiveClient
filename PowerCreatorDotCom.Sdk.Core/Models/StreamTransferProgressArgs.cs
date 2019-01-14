using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreatorDotCom.Sdk.Core.Models
{
    public class StreamTransferProgressArgs: EventArgs
    {
        private long _incrementTransferred;
        private long _total;
        private long _transferred;

        public StreamTransferProgressArgs(long incrementTransferred, long transferred, long total)
        {
            this._incrementTransferred = incrementTransferred;
            this._transferred = transferred;
            this._total = total;
        }

        public int PercentDone
        {
            get { return (int)((_transferred * 100) / _total); }
        }
        public long IncrementTransferred
        {
            get { return this._incrementTransferred; }
        }
        public long TransferredBytes
        {
            get { return _transferred; }
        }
        public long TotalBytes
        {
            get { return _total; }
        }
    }
}
