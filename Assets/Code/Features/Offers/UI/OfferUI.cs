using System;
using Code.Features.ConfigProviders.Data;
using Code.Features.Offers.Implementation;
using Code.Features.Offers.Service.Factory;
using Code.Services.Initialize.Ads;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Features.Offers.UI
{
    public class OfferUI : MonoBehaviour
    {
        public class OffersUIFactory : PlaceholderFactory<OfferUI>
        {
        }

        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _actionButton;

        private CompositeDisposable _disposables;
        private IAssetLoader _assetLoader;
        private IAdsService _adsService;
        private IPurchaseService _purchaseService;

        [Inject]
        public void Construct(IAssetLoader assetLoader, IAdsService adsService, IPurchaseService purchaseService)
        {
            _assetLoader = assetLoader;
            _adsService = adsService;
            _purchaseService = purchaseService;
        }

        public void Bind(Offer offer)
        {
            _disposables?.Dispose();
            _disposables = new CompositeDisposable();

            LoadIconAsync(offer.RewardIconPath).Forget();
            _titleText.text = offer.DisplayName;

            if (offer.IsPurchased)
            {
                _priceText.text = "Purchased";
                _actionButton.interactable = false;
            }
            else
            {
                // TODO: added metadata files for set price from stores
                _priceText.text = offer.PurchaseType == PurchaseType.RewardedAd
                    ? "Watch Ad"
                    : offer.Price.ToString("$0.00");

                _actionButton.interactable = true;
                _actionButton.OnClickAsObservable()
                    .Subscribe(_ => _purchaseService.Purchase(offer.Id))
                    .AddTo(_disposables);
            }
        }

        private async UniTaskVoid LoadIconAsync(string address)
        {
            try
            {
                var sprite = await _assetLoader.LoadAssetAsync<Sprite>(address);
                _iconImage.sprite = sprite;
            }
            catch (Exception exc)
            {
                Debug.LogError($"Failed to load icon '{address}': {exc}");
            }
        }

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
    }
}