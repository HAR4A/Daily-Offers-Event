using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

namespace Code.Services.Initialize.Firebase
{
    public class FirebaseInitializationService : IInitializeService, IFirebaseService
    {
        private const string OffersKey = "offers_data";

        public async UniTask InitializeAsync()
        {
            var depsTask = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (depsTask != DependencyStatus.Available)
            {
                Debug.LogError($"Firebase dependencies error: {depsTask}");
                return;
            }

            Debug.Log("FirebaseApp initialized");
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;

            await remoteConfig.SetDefaultsAsync(new System.Collections.Generic.Dictionary<string, object>
                {
                    { OffersKey, "" }
                })
                .ContinueWithOnMainThread(_ => Debug.Log("RemoteConfig defaults set"));

            try
            {
                await remoteConfig.FetchAsync(TimeSpan.Zero); //Can change Zero to 12 hours
                Debug.Log("RemoteConfig fetch completed");

                await remoteConfig.ActivateAsync();
                Debug.Log("RemoteConfig activated");
            }
            catch (Exception exc)
            {
                Debug.LogError($"RemoteConfig fetch/activate error: {exc}");
            }
        }
    }
}