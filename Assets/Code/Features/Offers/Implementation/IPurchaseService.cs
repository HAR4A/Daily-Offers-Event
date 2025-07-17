using System;

namespace Code.Features.Offers.Implementation
{
    public interface IPurchaseService
    {
        event Action<string> OnPurchase;
        void Purchase(string offerId);
    }
}