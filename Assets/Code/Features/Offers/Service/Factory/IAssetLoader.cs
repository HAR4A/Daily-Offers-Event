using Cysharp.Threading.Tasks;

namespace Code.Features.Offers.Service.Factory
{
    public interface IAssetLoader
    {
        UniTask<T> LoadAssetAsync<T>(string address);
    }
}