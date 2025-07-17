using Cysharp.Threading.Tasks;

namespace Code.Services.Initialize
{
    public interface IInitializeService
    {
        UniTask InitializeAsync();
    }
}