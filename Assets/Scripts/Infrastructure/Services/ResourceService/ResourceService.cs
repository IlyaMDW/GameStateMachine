using System;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Saving;

namespace Infrastructure.Services.ResourceService
{
    public class ResourceService : IResourceService, IDataSaveable<ResourceServiceSaveData>
    {
        public event Action OnAnyUpdated;
        public event Action<long> OnGoldUpdated;
        public event Action<long> OnDiamondsUpdated;

        private readonly IConditionalLoggingService _conditionalLoggingService;
        public string SaveId => SaveKeys.ResourceService;

        public ResourceServiceSaveData SaveData { get; set; }
        public ResourceServiceSaveData Default => new();
        
        private IAnalyticsLogService _analyticsLogService;

        public ResourceService(IConditionalLoggingService conditionalLoggingService, IAnalyticsLogService analyticsLogService)
        {
            _conditionalLoggingService = conditionalLoggingService;
            _analyticsLogService = analyticsLogService;
        }

        public long Gold
        {
            set
            {
                if (SaveData.IsInfinityMoneyCapacity)
                {
                    SaveData.Gold = value;
                }
                else
                {
                    if (value > SaveData.MaxGold) value = SaveData.MaxGold;
                    if (value < 0)
                    {
                        _conditionalLoggingService.LogError("Trying to set resource less than zero", LogTag.ResourceService);
                        value = 0;
                    }

                    SaveData.Gold = value;
                }

                OnGoldUpdated?.Invoke(value);
                OnAnyUpdated?.Invoke();
            }
            get => SaveData.Gold;
        }

        public long MaxGold => SaveData.MaxGold;

        public long Diamonds
        {
            set
            {
                if (value < 0)
                {
                    _conditionalLoggingService.LogError("Trying to set resource less than zero", LogTag.ResourceService);
                    value = 0;
                }

                SaveData.Diamonds = value;
                OnDiamondsUpdated?.Invoke(value);
                OnAnyUpdated?.Invoke();
            }
            get => SaveData.Diamonds;
        }
    }
}