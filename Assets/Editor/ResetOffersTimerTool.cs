using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class ResetOffersTimerTool
    {
        private const string UserDataFolderName = "UserData";
        private const string StateFileName = "EventState.json";

        [MenuItem("Tools/Reset Offers Timer")]
        private static void ResetOffersTimer()
        {
            string dir = Path.Combine(Application.persistentDataPath, UserDataFolderName);
            string path = Path.Combine(dir, StateFileName);

            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[ResetOffersTimer] Deleted state file at: {path}");
            }
            else
            {
                Debug.LogWarning($"[ResetOffersTimer] No state file found at: {path}");
            }
        }
    }
}