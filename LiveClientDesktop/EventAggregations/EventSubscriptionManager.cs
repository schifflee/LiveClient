using System;
using Microsoft.Practices.Prism.Events;

namespace LiveClientDesktop.EventAggregations
{
    public class EventSubscriptionManager
    {
        private readonly IEventAggregator _eventAggregator;
        public EventSubscriptionManager(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException("eventAggregator");
        }
        public SubscriptionToken Subscribe<EventType,EventContext>(SubscriptionToken subscriptionToken, Action<EventContext> eventHandler, Predicate<EventContext> eventFilter) where EventType : CompositePresentationEvent<EventContext>, new()
        {
            if (eventHandler == null)
                throw new ArgumentNullException("eventHandler");

            EventType e = _eventAggregator.GetEvent<EventType>();
            if (subscriptionToken != null)
            {
                e.Unsubscribe(subscriptionToken);
            }
            if (eventFilter != null)
            {
                subscriptionToken = e.Subscribe(eventHandler, ThreadOption.UIThread, false, eventFilter);
            }
            else
            {
                subscriptionToken = e.Subscribe(eventHandler, ThreadOption.UIThread, false);
            }
            return subscriptionToken;
        }
    }
}
