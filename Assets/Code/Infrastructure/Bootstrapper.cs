using System.Collections.Generic;
using Code.Services.Initialize;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [Inject] private IEnumerable<IInitializeService> _initializeServices;

        private async void Start()
        {
            if (_initializeServices == null)
            {
                Debug.LogError("Bootstrapper: _initializeServices == null! Проверьте SceneContext и Installers.");
                return;
            }

            await RunInitialization();
        }

        private async UniTask RunInitialization()
        {
            foreach (var service in _initializeServices)
            {
                try
                {
                    await service.InitializeAsync();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error initializing {service.GetType().Name}: {ex}");
                    return;
                }
            }

            Debug.Log("All SDK initialized.");
        }
    }
}