using System;
using System.Collections.Generic;
using Infrastructure.Services.Logging;
using Newtonsoft.Json;
using Zenject;

namespace Infrastructure.Services.Saving
{
    public abstract class BaseSaveService : ISaveService
    {
        public abstract event Action OnBeforeStoreSaveFiles;
        
        protected Dictionary<string, object> _readyToSaveDictionary = new();

        protected readonly IConditionalLoggingService _loggingService;

        protected string _cachedSaveFileName;
        protected abstract string _defaultFileName { get; }

        public abstract void LoadSaveFile(bool useDefaultFileName = true, string fileName = null);

        public abstract void StoreSaveFile(bool useDefaultFileName = true, string fileName = null);
        public abstract void RemoveDefaultSaveFile();

        [Inject]
        protected BaseSaveService(IConditionalLoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void Load<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class
        {
            if (_readyToSaveDictionary.TryGetValue(dataSaveable.SaveId, out var value))
            {
                dataSaveable.SaveData = JsonConvert.DeserializeObject<TSave>(value.ToString()) ?? dataSaveable.Default;
            }
            else
            {
                dataSaveable.SaveData = dataSaveable.Default;
            }
        }


        public void AddToSave<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class
        {
            if (_readyToSaveDictionary.ContainsKey(dataSaveable.SaveId))
            {
                _readyToSaveDictionary[dataSaveable.SaveId] = dataSaveable.SaveData;
                return;
            }

            _readyToSaveDictionary.Add(dataSaveable.SaveId, dataSaveable.SaveData);
        }

        public void LoadAndAddToSave<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class
        {
            Load(dataSaveable);
            AddToSave(dataSaveable);
        }
    }
}