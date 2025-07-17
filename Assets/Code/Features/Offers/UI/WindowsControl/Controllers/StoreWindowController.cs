using Code.Features.Offers.Implementation;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Features.Offers.UI.WindowsControl.Controllers
{
    //TODO Remove MonoBehaviour and added event for click on button, class must have a reference for View 
    public class StoreWindowController : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        
        [Inject] private OfferEvent _offerEvent;
        
        private void OnEnable()
        {
            _offerEvent.OnStateChanged += RefreshButton;
            RefreshButton();
        }

        private void OnDestroy()
        {
            _offerEvent.OnStateChanged -= RefreshButton;
        }

        private void RefreshButton()
        {
            _openButton.interactable = _offerEvent.IsActive();
        }
    }
}