using System;
using UnityEngine;

namespace Code.Features.ConfigProviders.Data
{
    public static class PurchaseTypeParser
    {
        public static PurchaseType Parse(string raw)
        {
            if (string.IsNullOrEmpty(raw))
                return PurchaseType.InAppPurchase;

            if (Enum.TryParse(raw, true, out PurchaseType pt))
                return pt;

            Debug.LogWarning($"[PurchaseTypeParser] Unknown PurchaseType '{raw}', defaulted to InAppPurchase.");
            return PurchaseType.InAppPurchase;
        }
    }
}