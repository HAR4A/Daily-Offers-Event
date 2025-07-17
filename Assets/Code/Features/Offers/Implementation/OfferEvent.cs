using System;
using System.Collections.Generic;
using System.Linq;
using Code.Features.ConfigProviders.Data;
using Code.Features.ConfigProviders.Implementation;
using Code.Features.DataProviders.Implementation;
using Code.Features.DataProviders.Implementation.States;
using Code.Features.Timer.Implementation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Features.Offers.Implementation
{
    public class OfferEvent : IRemainingTimeProvider
    {
        public event Action<OfferEvent> OnOffersUpdated;
        public event Action OnEventExpired;
        public event Action OnStateChanged;

        private IConfigProvider _configProvider;
        private IDataStorage _storage;
        private ITimerService _timerService;
        private IPurchaseService _purchaseService;

        private EventState _state;
        private List<Offer> _offers;
        private List<OfferConfig> _configs;

        private long _eventDurationSeconds;

        [Inject]
        public OfferEvent(IConfigProvider configProvider,
            IDataStorage storage,
            ITimerService timerService,
            IPurchaseService purchaseService)
        {
            _configProvider = configProvider;
            _storage = storage;
            _timerService = timerService;
            _purchaseService = purchaseService;
        }
        
        public long GetRemainingTime() =>
            (_state.ActivationTime + _eventDurationSeconds)
            - DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public IReadOnlyList<Offer> GetOffers() => _offers;

        public void OnExpired()
        {
            _timerService.Unregister(this);
            OnEventExpired?.Invoke();
            OnStateChanged?.Invoke();
            CompleteEvent();
        }

        public bool IsActive()
        {
            if (_state == null || _offers == null)
                return false;

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            bool withinWindow = now >= _state.ActivationTime
                                && now <= _state.ActivationTime + _eventDurationSeconds;

            bool hasUnpurchased = _offers.Any(o => !o.IsPurchased);
            return withinWindow && hasUnpurchased;
        }

        public void SetConfigs(List<OfferConfig> configs)
        {
            _configs = configs;
        }

        public void StartEvent()
        {
            ActivateAsync().Forget();
        }

        private async UniTask ActivateAsync()
        {
            if (_configs == null || _configs.Count == 0)
            {
                Debug.LogError("[OfferEvent] ActivateAsync: no offer configs loaded!");
                return;
            }

            _eventDurationSeconds = _configs.First().DurationSeconds;
            _state = await _storage.LoadState<EventState>();
            if (_state == null)
            {
                Debug.LogError("[OfferEvent] ActivateAsync: failed to load EventState (null returned)");
                _state = new EventState();
            }

            //место проверки первичного запуска 
            if (_state.ActivationTime == 0) //тут хз на счет 0, можно в константу вывести и подписать как-нибудь явно...
            {
                _state.ActivationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                await _storage.SaveState(_state);
            }

            try
            {
                _purchaseService.OnPurchase += HandlePurchase;
                _timerService.Register(this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[OfferEvent] ActivateAsync: failed to subscribe/register: {ex}");
                throw;
            }

            CreatOffersFromConfigs(_configs);
            OnOffersUpdated?.Invoke(this);
            OnStateChanged?.Invoke();
        }

        private async void HandlePurchase(string offerId)
        {
            if (_state.PurchasedOfferIds.Contains(offerId))
            {
                Debug.LogWarning($"[OfferEvent] HandlePurchase: offer '{offerId}' already purchased, skipping");
                return;
            }

            _state.PurchasedOfferIds.Add(offerId);
            await _storage.SaveState(_state);

            var offer = _offers.FirstOrDefault(o => o.Id == offerId);
            if (offer == null)
            {
                Debug.LogError($"[OfferEvent] HandlePurchase: no Offer instance found for id='{offerId}'");
            }
            else
            {
                offer.IsPurchased = true;
                OnOffersUpdated?.Invoke(this);
                OnStateChanged?.Invoke();
            }

            if (_offers.All(o => o.IsPurchased))
            {
                OnExpired();
            }
        }

        private void CreatOffersFromConfigs(List<OfferConfig> configs)
        {
            _offers = configs
                .Select(cfg =>
                {
                    var offer = new Offer(cfg);
                    if (_state.PurchasedOfferIds.Contains(cfg.Id))
                        offer.IsPurchased = true;
                    return offer;
                })
                .ToList();
        }

        private void CompleteEvent()
        {
            _timerService.Unregister(this);
            //place for ui events
        }
    }
}