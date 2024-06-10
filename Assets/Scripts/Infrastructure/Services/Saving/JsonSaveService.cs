using System;
using System.Collections.Generic;
using System.IO;
using Infrastructure.Services.Logging;
using Newtonsoft.Json;
using UnityEngine;

namespace Infrastructure.Services.Saving
{
    public class JsonSaveService : BaseSaveService
    {
        public override event Action OnBeforeStoreSaveFiles;
        private bool _isSavingProhibited;
        protected override string _defaultFileName => "jsonDefaultSave";

        public JsonSaveService(IConditionalLoggingService loggingService) : base(loggingService)
        {
        }

        public override void LoadSaveFile(bool useDefaultFileName = true, string fileName = null)
        {
            if (useDefaultFileName) fileName = _defaultFileName;

            var path = $"{Application.persistentDataPath}/{fileName}.txt";

            if (!File.Exists(path))
            {
                _loggingService.Log("No game data to load", LogTag.SaveService);
                return;
            }

            _cachedSaveFileName = fileName;

            var file = File.ReadAllText(path);

            _readyToSaveDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(file);

            _loggingService.Log($"Game data loaded! From path: {path} \nContent: \n{file}", LogTag.SaveService);
        }

        public override void StoreSaveFile(bool useDefaultFileName = true, string fileName = null)
        {
            if (_isSavingProhibited) return;
            OnBeforeStoreSaveFiles?.Invoke();
            
            if (useDefaultFileName || _cachedSaveFileName == null) fileName = _defaultFileName;
            else if (fileName == null && _cachedSaveFileName != null) fileName = _cachedSaveFileName;

            var path = $"{Application.persistentDataPath}/{fileName}.txt";
            var serializedObject = JsonConvert.SerializeObject(_readyToSaveDictionary, Formatting.Indented);
            File.WriteAllText(path, serializedObject);
            _loggingService.Log($"Game data saved! At path: \n{path} \nContent: \n{serializedObject}", LogTag.SaveService);
        }

        public override void RemoveDefaultSaveFile()
        {
            _isSavingProhibited = true;
            var path = $"{Application.persistentDataPath}/{_defaultFileName}.txt";
            File.Delete(path);
        }
    }
}