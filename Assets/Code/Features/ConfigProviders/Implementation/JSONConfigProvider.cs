using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Code.Features.ConfigProviders.Data;
using Code.Features.ConfigProviders.Implementation.ConfigsRepository;
using Cysharp.Threading.Tasks;

namespace Code.Features.ConfigProviders.Implementation
{
    public class JsonConfigProvider : IConfigProvider
    {
        private readonly IEnumerable<IConfigRepository> _repositories;

        public JsonConfigProvider(IEnumerable<IConfigRepository> repositories)
        {
            _repositories = repositories;
        }

        public async UniTask<List<OfferConfig>> LoadConfigsAsync()
        {
            string jsonText = null;

            foreach (var repo in _repositories)
            {
                jsonText = await repo.GetOfferJson("Offers.json");
                if (!string.IsNullOrEmpty(jsonText))
                {
                    Debug.Log($"[JsonConfigProvider] Loaded JSON from {repo.GetType().Name}.");
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(jsonText))
            {
                Debug.LogError("[JsonConfigProvider] Failed to load JSON from all repositories.");
                return new List<OfferConfig>();
            }

            string wrapped = $"{{\"Offers\":{jsonText}}}";

            OffersConfig wrapper;
            try
            {
                wrapper = JsonUtility.FromJson<OffersConfig>(wrapped);
            }
            catch (Exception exc)
            {
                Debug.LogError($"[JsonConfigProvider] JSON deserialization error: {exc}");
                return new List<OfferConfig>();
            }

            if (wrapper?.Offers == null)
            {
                Debug.LogError("[JsonConfigProvider] Deserialized wrapper is null or Offers == null.");
                return new List<OfferConfig>();
            }

            return wrapper.Offers;
        }
    }
}