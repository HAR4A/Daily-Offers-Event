using System;
using System.Collections.Generic;

namespace Code.Features.DataProviders.Implementation.States
{
    [Serializable]
    public class EventState
    {
        public long ActivationTime;
        public List<string> PurchasedOfferIds { get; set; } = new List<string>();
    }
}