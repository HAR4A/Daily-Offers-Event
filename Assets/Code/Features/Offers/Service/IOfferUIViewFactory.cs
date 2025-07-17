using Code.Features.Offers.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Features.Offers.Service
{
    public interface IOfferUIViewFactory
    {
        UniTask<OfferUI> CreateAsync(Transform parent);
    }
}