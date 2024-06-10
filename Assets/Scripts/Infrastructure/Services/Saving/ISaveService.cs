using System;

namespace Infrastructure.Services.Saving
{
    public interface ISaveService
    {
        public event Action OnBeforeStoreSaveFiles;
        
        void AddToSave<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class;

        void Load<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class;
        void LoadAndAddToSave<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class;

        void LoadSaveFile(bool useDefaultFileName = true, string fileName = null);
        void StoreSaveFile(bool useDefaultFileName = true, string fileName = null);
        void RemoveDefaultSaveFile();
    }
}