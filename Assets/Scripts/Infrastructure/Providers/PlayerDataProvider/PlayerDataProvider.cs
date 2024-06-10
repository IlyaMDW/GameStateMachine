using System;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Logging;
using Infrastructure.Services.ResourceService;
using Infrastructure.Services.Saving;

namespace Infrastructure.Providers.PlayerDataProvider
{
    public class PlayerDataProvider : IPlayerDataProvider, IDataSaveable<PlayerDataProvider.Save>
    {
        [Serializable]
        public class Save
        {
            public long killedMobs;
            public long exp;
        }

        public event Action<int> OnGoldChangedByEntity;
        public event Action<int> OnDiamondsChangedByEntity;
        public event Action<long> OnExperienceUpdated;

        private ResourceService _resourceService;
        private IConditionalLoggingService _loggingService;
        private IAnalyticsLogService _analyticsLogService;
        private ISaveService _saveService;

        public string SaveId => SaveKeys.PlayerDataProvider;
        public Save SaveData { get; set; }

        public Save Default => new()
        {

        };

        public PlayerDataProvider(ResourceService resourceService, IConditionalLoggingService loggingService, IAnalyticsLogService analyticsLogService, ISaveService saveService)
        {
            _analyticsLogService = analyticsLogService;
            _loggingService = loggingService;
            _resourceService = resourceService;
            _saveService = saveService;
        }
    }
}