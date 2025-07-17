using System;
using System.Collections.Generic;
using System.Linq;
using Code.Features.ConfigProviders.Data;
using Code.Services.Initialize.Ads;
using Code.Services.Initialize.IAP;
using UnityEngine;

namespace Code.Features.Offers.Implementation
{
    public class PurchaseService : IPurchaseService
    {
        public event Action<string> OnPurchase;

        private List<OfferConfig> _configs;
        private readonly Dictionary<PurchaseType, Action<OfferConfig>> _handlers;

        private readonly IAdsService _adsService;
        private readonly IIAPUnityPurchaseService _iapUnityService;

        public PurchaseService(IAdsService adsService, IIAPUnityPurchaseService iapUnityService)
        {
            _adsService = adsService;
            _iapUnityService = iapUnityService;

            _handlers = new Dictionary<PurchaseType, Action<OfferConfig>>
            {
                [PurchaseType.RewardedAd] = config => _adsService.ShowRewardedAd(config.Id),
                [PurchaseType.InAppPurchase] = config => _iapUnityService.PurchaseProduct(config.ProductId)
            };

            _adsService.OnAdCompleted += id => OnPurchase?.Invoke(id);
            _iapUnityService.OnPurchaseSuccess += prodId =>
            {
                var config = _configs.FirstOrDefault(c => c.ProductId == prodId);
                if (config != null)
                    OnPurchase?.Invoke(config.Id);
            };
        }

        public void SetConfigs(List<OfferConfig> configs)
        {
            _configs = configs;
        }

        public void Purchase(string offerId)
        {
            var config = _configs.FirstOrDefault(c => c.Id == offerId);
            if (config == null)
            {
                Debug.LogError($"[PurchaseService] Purchase failed: no OfferConfig found for id='{offerId}'");
                return;
            }
            
            var type = PurchaseTypeParser.Parse(config.PurchaseType);

            if (_handlers.TryGetValue(type, out var handler))
                handler(config);
            else
                Debug.LogError($"[PurchaseService] No handler for PurchaseType '{type}' on offer '{config.Id}'");
        }
    }
}