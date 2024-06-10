using System;
using Configs;

namespace Infrastructure.Services.ResourceService
{
    [Serializable]
    public class ResourceServiceSaveData
    {
#if DEV
        public long Diamonds = Remote.ResourceServiceConfig.DefaultDiamondsAtStart;
        public long Gold = Remote.ResourceServiceConfig.DefaultGoldAtStart;
        public long MaxGold = Remote.ResourceServiceConfig.DefaultMaxGold;
        public bool IsInfinityMoneyCapacity = false;
#else
        public long Diamonds = Remote.ResourceServiceConfig.DefaultDiamondsAtStart;
        public long Gold = Remote.ResourceServiceConfig.DefaultGoldAtStart;
        public long MaxGold = Remote.ResourceServiceConfig.DefaultMaxGold;
        public bool IsInfinityMoneyCapacity = false;
#endif
    }
}