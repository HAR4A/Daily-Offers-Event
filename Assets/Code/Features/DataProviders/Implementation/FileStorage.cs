using System;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using Application = UnityEngine.Device.Application;
using Code.Features.DataProviders.Implementation.States;

namespace Code.Features.DataProviders.Implementation
{
    public class FileStorage : IDataStorage
    {
        private const string UserDataDirectoryName = "UserData";
        private readonly string _directoryPath;

        public FileStorage()
        {
            _directoryPath = Path.Combine(Application.persistentDataPath, UserDataDirectoryName);
            if (!Directory.Exists(_directoryPath))
                Directory.CreateDirectory(_directoryPath);
        }

        public async UniTask<T> LoadState<T>() where T : EventState, new()
        {
            var filePath = GetFilePath<T>();

            if (!File.Exists(filePath))
                return new T();
            try
            {
                var json = await File.ReadAllTextAsync(filePath).AsUniTask();
                var state = JsonConvert.DeserializeObject<T>(json);
                return state ?? new T();
            }
            catch (Exception exception)
            {
                Debug.LogError($"[FileStorage] Error loading {typeof(T).Name}: {exception}");
                return new T();
            }
        }

        public async UniTask SaveState<T>(T state) where T : EventState
        {
            var filePath = GetFilePath<T>();

            try
            {
                var json = JsonConvert.SerializeObject(state, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, json).AsUniTask();
            }
            catch (Exception exception)
            {
                Debug.LogError($"[FileStorage] Error saving {typeof(T).Name}: {exception}");
            }
        }

        private string GetFilePath<T>() where T : EventState
        {
            var fileName = $"{typeof(T).Name}.json";
            return Path.Combine(_directoryPath, fileName);
        }
    }
}