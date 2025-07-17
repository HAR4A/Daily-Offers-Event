using Code.Features.DataProviders.Implementation.States;
using Cysharp.Threading.Tasks;

namespace Code.Features.DataProviders.Implementation
{
    public interface IDataStorage
    {
        UniTask<T> LoadState<T>() where T : EventState, new();
        UniTask SaveState<T>(T state) where T : EventState;
    }
}