using Code.Features.ConfigProviders.Data;

namespace Code.Features.Offers.Implementation
{
    public class Offer
    {
        public string Id { get; }
        public string DisplayName { get; }
        public float Price { get; }
        public string RewardDescriptionKey { get; }
        public string RewardIconPath { get; }
        public bool IsPurchased { get; set; }
        public string ProductId { get; }
        public PurchaseType PurchaseType { get; }

        public Offer(OfferConfig config)
        {
            Id = config.Id;
            DisplayName = config.DisplayName;
            Price = config.Price;
            RewardDescriptionKey = config.RewardDescriptionKey;
            RewardIconPath = config.RewardIconPath;
            IsPurchased = false;
            ProductId = config.ProductId;
            PurchaseType = PurchaseTypeParser.Parse(config.PurchaseType);
        }
    }
}