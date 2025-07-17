using Code.Features.Offers.Service.Factory;
using Code.Features.Offers.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Features.Offers.Service
{
    public class AddressablesOfferUIViewFactory : IOfferUIViewFactory
    {
        private const string OfferUIAddress = "GUI Kit DarkStone/Prefabs/OfferPrefab";

        private readonly IAssetLoader _assetLoader;
        private readonly DiContainer _container;

        [Inject]
        public AddressablesOfferUIViewFactory(IAssetLoader assetLoader, DiContainer container)
        {
            _assetLoader = assetLoader;
            _container = container;
        }

        public async UniTask<OfferUI> CreateAsync(Transform parent)
        {
            var offerPrefab = await _assetLoader.LoadAssetAsync<GameObject>(OfferUIAddress);
            var spawn = Object.Instantiate(offerPrefab, parent, false);
            _container.InjectGameObject(spawn);
            return spawn.GetComponent<OfferUI>();
        }
    }
}