using Code.Features.Offers.Implementation;
using Code.Features.Offers.Service;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Features.Offers.UI
{
    public class WindowOfferView : MonoBehaviour
    {
        [SerializeField] private Transform _contentParent;
        [SerializeField] private Button _closeButton;

        private OfferEvent _offerEvent;
        private IOfferUIViewFactory _uiFactory;

        [Inject]
        private void Construct(OfferEvent offerEvent, IOfferUIViewFactory uiFactory)
        {
            _offerEvent = offerEvent;
            _uiFactory = uiFactory;
        }

        private void Awake()
        {
            _offerEvent.OnOffersUpdated += OnOffersUpdated;
            _offerEvent.OnEventExpired  += HandleEventExpired;
            
            _closeButton.onClick.AddListener(() =>
                gameObject.SetActive(false)
            );
        }

        private void OnEnable()
        {
            RefreshUIAsync().Forget();
        }

        private void OnDestroy()
        {
            _offerEvent.OnOffersUpdated -= OnOffersUpdated;
            _offerEvent.OnEventExpired  -= HandleEventExpired;
        }

        private void OnOffersUpdated(OfferEvent ev)
        {
            RefreshUIAsync().Forget();
        }

        private async UniTaskVoid RefreshUIAsync()
        {
            foreach (Transform child in _contentParent)
                Destroy(child.gameObject);

            foreach (var offer in _offerEvent.GetOffers())
            {
                var ui = await _uiFactory.CreateAsync(_contentParent);
                ui.transform.SetParent(_contentParent, false);
                ui.Bind(offer);
            }
        }
        private void HandleEventExpired()
        {
            gameObject.SetActive(false);
        }
    }
}