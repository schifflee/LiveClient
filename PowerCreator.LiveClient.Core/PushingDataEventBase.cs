using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core
{
    public abstract class PushingDataEventBase<EventDataContext> : IPushingDataPushingData<EventDataContext>
    {
        private event Action<EventDataContext> _pushingData;
        private object syncState = new object();
        public event Action<EventDataContext> PushingData
        {
            add
            {
                lock (syncState)
                {
                    OnSubscribe();
                    _pushingData -= value;
                    _pushingData += value;
                }
            }
            remove
            {
                lock (syncState)
                {
                    _pushingData -= value;
                    if (_pushingData == null)
                        OnAllUnSubscribe();
                }
            }
        }
        protected abstract void OnSubscribe();

        protected abstract void OnAllUnSubscribe();

        public void Pushing(EventDataContext context)
        {
            _pushingData?.Invoke(context);
        }
    }
}
