using System.Collections.Generic;
using Code.Features.ConfigProviders.Data;
using Cysharp.Threading.Tasks;

namespace Code.Features.ConfigProviders.Implementation
{
    public interface IConfigProvider
    {
        UniTask<List<OfferConfig>> LoadConfigsAsync();
    }
}