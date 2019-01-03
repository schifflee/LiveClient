using System;
using System.Collections.Generic;

namespace PowerCreator.LiveClient.Core
{
    public class Unsubscriber<T> : IDisposable
    {
        private List<IObserver<T>> _observers;
        private IObserver<T> _observer;
        private Action<List<IObserver<T>>> _unSubscriberCompletedCallBack;
        private string id;
        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer, Action<List<IObserver<T>>> unSubscriberCompletedCallBack)
        {
            id = Guid.NewGuid().ToString();
            _observers = observers;
            _observer = observer;
            _unSubscriberCompletedCallBack = unSubscriberCompletedCallBack;
        }
        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);

            _unSubscriberCompletedCallBack?.Invoke(_observers);
        }
        public override string ToString()
        {
            return id;
        }
    }
}
