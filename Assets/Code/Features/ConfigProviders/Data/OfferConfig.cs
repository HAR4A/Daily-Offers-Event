using System;

namespace Code.Features.ConfigProviders.Data
{
    [Serializable]
    public class OfferConfig
    {
        public string Id;
        public string DisplayName;
        public float Price;
        public string RewardDescriptionKey;
        public string RewardIconPath;
        public long DurationSeconds;
        public string ProductId;
        public string PurchaseType;
    }
}