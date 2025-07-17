using UnityEngine;
using Zenject;
using Code.Services.Initialize.Ads;
using Code.Services.Initialize.Firebase;
using Code.Services.Initialize.IAP;

namespace Code.Infrastructure.Installers
{
    public class SdkInstaller : MonoInstaller
    {
        [Header("Unity Ads")] [SerializeField] private string _androidGameId;
        [SerializeField] private string _iOSGameId;
        [SerializeField] private bool _testMode;

        [Header("Rewarded Video Ad Units")]
        [SerializeField] private string _androidRewardedUnitId;
        [SerializeField] private string _iOSRewardedUnitId;
        public override void InstallBindings()
        {
            string gameId =
#if UNITY_ANDROID
                _androidGameId;
#elif UNITY_IOS
                _iOSGameId;
#elif UNITY_EDITOR
                _androidGameId;
#else
                null;
#endif

            Container.BindInstance(gameId).WithId("UnityAdsGameId");
            Container.BindInstance(_testMode).WithId("AdsTestMode");

            Container.BindInterfacesAndSelfTo<AdsInitializationService>().AsSingle();
            Container.BindInterfacesAndSelfTo<IAPUnityInitializationService>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseInitializationService>().AsSingle();
            
            Container.BindInstance(_androidRewardedUnitId).WithId("Rewarded_Android");
            Container.BindInstance(_iOSRewardedUnitId).WithId("Rewarded_iOS");
        }
    }
}