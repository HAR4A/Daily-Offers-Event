using Code.Features.ConfigProviders.Implementation;
using Code.Features.Offers.Implementation;
using Code.Services.Initialize;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Features.Offers.Service
{
    public class OffersInitializationService : IInitializeService
    {
       private readonly IConfigProvider _configProvider;
       private readonly PurchaseService _purchaseService;
       private readonly OfferEvent _offerEvent;

        [Inject]
        public OffersInitializationService(
            IConfigProvider configProvider,
            PurchaseService purchaseService,
            OfferEvent offerEvent)
        {
            _configProvider = configProvider;
            _purchaseService = purchaseService;
            _offerEvent = offerEvent;
        }

        public async UniTask InitializeAsync()
        {
            var configs = await _configProvider.LoadConfigsAsync();
            Debug.Log($"[OffersInit] Loaded {configs?.Count ?? 0} offers");

            _purchaseService.SetConfigs(configs);
            _offerEvent.SetConfigs(configs);
            _offerEvent.StartEvent();
        }
    }
}