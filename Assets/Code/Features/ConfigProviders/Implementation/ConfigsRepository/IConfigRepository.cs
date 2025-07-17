using Cysharp.Threading.Tasks;

namespace Code.Features.ConfigProviders.Implementation.ConfigsRepository
{
    public interface IConfigRepository
    {
        UniTask<string> GetOfferJson(string jsonName);
    }
}