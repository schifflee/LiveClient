using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core
{
    public abstract class DevicDataRecipient<T> : IObserver<T>
    {
        protected IDisposable unsubscriber;
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(T value);
        public virtual void Subscribe(IObservable<T> provider)
        {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }
        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
    }
}
