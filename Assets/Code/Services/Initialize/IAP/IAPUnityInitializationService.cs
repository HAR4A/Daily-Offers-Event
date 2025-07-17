using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Code.Services.Initialize.IAP
{
    public class IAPUnityInitializationService : IInitializeService, IIAPUnityPurchaseService, IStoreListener
    {
        public event Action<string> OnPurchaseSuccess;
        
        IStoreController _controller;
        IExtensionProvider _extensions;
        private readonly UniTaskCompletionSource _tcs = new();

        
        public UniTask InitializeAsync()
        {
            var module = StandardPurchasingModule.Instance();
            var builder = ConfigurationBuilder.Instance(module);
            
            builder.AddProduct("com.HAR4A.DailyOffers.potion_pack", ProductType.Consumable);
            builder.AddProduct("com.HAR4A.DailyOffers.shield_pack", ProductType.Consumable);
            
            UnityPurchasing.Initialize(this, builder);
            return _tcs.Task;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;
            Debug.Log("Unity IAP initialized");
            _tcs.TrySetResult();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"IAP init failed: {error}");
            _tcs.TrySetResult();
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"IAP Init Failed: {error}, Message: {message}");
            _tcs.TrySetResult();
        }

        public void PurchaseProduct(string productId)
        {
            if (_controller == null)
            {
                Debug.LogError("IAP not initialized yet");
                return;
            }
            _controller.InitiatePurchase(productId);
        }

        public Product GetProduct(string productId)
        {
            if (_controller == null)
            {
                Debug.LogWarning("IAP not initialized when requesting product");
                return null;
            }
            return _controller.products.WithID(productId);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            OnPurchaseSuccess?.Invoke(purchaseEvent.purchasedProduct.definition.id);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogWarning($"IAP failed: {product.definition.id}, {failureReason}");
        }
    }
}