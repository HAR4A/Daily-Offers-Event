using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using UnityEngine;

namespace Code.Features.ConfigProviders.Implementation.ConfigsRepository
{
    public class FirebaseConfigRepository : IConfigRepository
    {
        private readonly string _remoteKey;

        public FirebaseConfigRepository()
        {
            _remoteKey = "offers_data";
        }

        public UniTask<string> GetOfferJson(string jsonName)
        {
            try
            {
                var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
                return UniTask.FromResult(remoteConfig.GetValue(_remoteKey).StringValue);
            }
            catch (System.Exception exc)
            {
                Debug.LogWarning($"[FirebaseConfigRepository] Error getting remote JSON: {exc}");
                return UniTask.FromResult<string>(null);
            }
        }
    }
}