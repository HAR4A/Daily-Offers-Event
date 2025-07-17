using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Features.ConfigProviders.Implementation.ConfigsRepository
{
    public class LocalConfigRepository : IConfigRepository
    {
        public UniTask<string> GetOfferJson(string jsonName)
        {
            string path = Path.Combine(Application.streamingAssetsPath, jsonName);

            if (!File.Exists(path))
            {
                Debug.LogError($"[LocalConfigRepository] File not found: {path}");
                return UniTask.FromResult<string>(null);
            }

            return File.ReadAllTextAsync(path).AsUniTask();
        }
    }
}