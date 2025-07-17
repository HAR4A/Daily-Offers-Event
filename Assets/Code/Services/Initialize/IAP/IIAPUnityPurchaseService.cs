using System;

namespace Code.Services.Initialize.IAP
{
    public interface IIAPUnityPurchaseService
    {
        event Action<string> OnPurchaseSuccess;
        void PurchaseProduct(string productId);
        
    }
}