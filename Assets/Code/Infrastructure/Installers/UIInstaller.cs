using Code.Features.Offers.Service;
using Code.Features.Offers.Service.Factory;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetLoader>().To<AddressablesAssetLoader>().AsSingle();
            Container.Bind<IOfferUIViewFactory>().To<AddressablesOfferUIViewFactory>().AsSingle();
        }
    }
}