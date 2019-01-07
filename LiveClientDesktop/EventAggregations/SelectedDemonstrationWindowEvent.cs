using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Events;

namespace LiveClientDesktop.EventAggregations
{
    public class SelectedDemonstrationWindowEvent: CompositePresentationEvent<PreviewWindowInfo>
    {
    }
}
