﻿using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Events;
using System.Collections.Generic;

namespace LiveClientDesktop.EventAggregations
{
    public class LoadWarmVideoCompletedEvent:CompositePresentationEvent<List<PresentationInfo>>
    {
    }
}
