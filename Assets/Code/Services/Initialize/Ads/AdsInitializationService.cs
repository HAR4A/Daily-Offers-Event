using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace Code.Services.Initialize.Ads
{
    public class AdsInitializationService : IInitializeService, IAdsService, IUnityAdsInitializationListener,
        IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public event Action<string> OnAdCompleted;
        
        readonly string _gameId;
        readonly bool _testMode;
        readonly string _rewardedUnitId;
        private bool _adLoaded;
        private string _pendingOfferId;

        private readonly UniTaskCompletionSource _tcs = new();

        [Inject]
        public AdsInitializationService(
            [Inject(Id = "UnityAdsGameId")] string gameId,
            [Inject(Id = "AdsTestMode")] bool testMode,
            [Inject(Id = "Rewarded_Android")] string androidUnit,
            [Inject(Id = "Rewarded_iOS")] string iosUnit)
        {
            _gameId = gameId;
            _testMode = testMode;

#if UNITY_ANDROID
            _rewardedUnitId = androidUnit;
#elif UNITY_IOS
        _rewardedUnitId = iosUnit;
#else
        _rewardedUnitId = null;
#endif
        }

        public UniTask InitializeAsync()
        {
            if (string.IsNullOrEmpty(_gameId) || !Advertisement.isSupported)
            {
                Debug.LogWarning("Unity Ads: unsupported or no gameId.");
                return UniTask.CompletedTask;
            }

            Advertisement.Initialize(_gameId, _testMode, this);
            return _tcs.Task;
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialized");
            _tcs.TrySetResult();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"Unity Ads init failed: {error} {message}");
            _tcs.TrySetResult();
        }

        public void LoadRewardedAd()
        {
            _adLoaded = false;
            if (!string.IsNullOrEmpty(_rewardedUnitId))
                Advertisement.Load(_rewardedUnitId, this);
        }

        public void ShowRewardedAd(string offerId)
        {
            if (_adLoaded)
            {
                _pendingOfferId = offerId;
                Advertisement.Show(_rewardedUnitId, this);
            }
            else
            {
                _pendingOfferId = offerId;
                Advertisement.Load(_rewardedUnitId, this);
            }
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (placementId != _rewardedUnitId)
                return;
            
            if (string.IsNullOrEmpty(_pendingOfferId))
                return;
            
            _adLoaded = true;
            
            // TODO Remove this logs after fixed IAP purchase 
            Debug.Log($"[Ads] Ad loaded for offer '{_pendingOfferId}'");

            Advertisement.Show(_rewardedUnitId, this);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"Failed to load ad {placementId}: {error} – {message}");
            _pendingOfferId = null;
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId == _rewardedUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                OnAdCompleted?.Invoke(_pendingOfferId);
            }

            _pendingOfferId = null;
            _adLoaded = false;
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogError($"Show failure {error}:{message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }
    }
}