using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Code.Features.Offers.Service.Factory
{
    public class AddressablesAssetLoader : IAssetLoader
    {
        public async UniTask<T> LoadAssetAsync<T>(string address)
        {
            var handle = Addressables.LoadAssetAsync<T>(address);
            try
            {
                T asset = await handle.Task;
                return asset;
            }
            finally
            {
                Addressables.Release(handle);
            }
        }
    }
}