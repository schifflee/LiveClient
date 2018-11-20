using PowerCreator.LiveClient.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.VideoDevice
{
    public abstract class VideoDeviceGraphicsDataRecipient : IObserver<VideoDeviceData>
    {
        private IDisposable unsubscriber;
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(VideoDeviceData value);

        public virtual void Subscribe(IObservable<VideoDeviceData> provider)
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
