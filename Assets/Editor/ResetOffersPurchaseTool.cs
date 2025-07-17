using System.IO;
using Code.Features.DataProviders.Implementation.States;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class ResetOffersPurchaseTool
    {
        private const string UserDataFolderName = "UserData";
        private const string StateFileName = "EventState.json";

        [MenuItem("Tools/Reset Purchased Offers")]
        private static void ResetPurchasedOffers()
        {
            string dir = Path.Combine(Application.persistentDataPath, UserDataFolderName);
            string file = Path.Combine(dir, StateFileName);

            if (!File.Exists(file))
            {
                Debug.LogWarning($"[ResetPurchasedOffers] No state file found at: {file}");
                return;
            }

            var json = File.ReadAllText(file);
            var state = JsonUtility.FromJson<EventState>(json);

            state.PurchasedOfferIds.Clear();
            state.ActivationTime = 0;
            
            var newJson = JsonUtility.ToJson(state, prettyPrint: true);
            File.WriteAllText(file, newJson);

            Debug.Log($"[ResetPurchasedOffers] Cleared PurchasedOfferIds in: {file}");
        }
    }
}