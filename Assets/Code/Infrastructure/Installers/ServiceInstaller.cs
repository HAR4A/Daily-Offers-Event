using Code.Features.ConfigProviders.Implementation;
using Code.Features.ConfigProviders.Implementation.ConfigsRepository;
using Code.Features.DataProviders.Implementation;
using Code.Features.Offers.Implementation;
using Code.Features.Offers.Service;
using Code.Features.Timer.Implementation;
using Code.Services.Initialize;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IConfigProvider>().To<JsonConfigProvider>().AsSingle();
            Container.Bind<IInitializeService>().To<OffersInitializationService>().AsSingle();
            Container.Bind<IDataStorage>().To<FileStorage>().AsSingle();
            Container.BindInterfacesAndSelfTo<PurchaseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimerService>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesAndSelfTo<OfferEvent>().AsSingle();
            Container.Bind<IConfigRepository>().To<FirebaseConfigRepository>().AsSingle();
            Container.Bind<IConfigRepository>().To<LocalConfigRepository>().AsSingle();
        }
    }
}