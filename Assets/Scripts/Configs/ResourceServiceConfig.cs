using System;
using Newtonsoft.Json;

namespace Configs
{
    [Serializable]
    public class ResourceServiceConfig : IConfig
    {
        [JsonProperty] public long DefaultDiamondsAtStart;
        [JsonProperty] public long DefaultGoldAtStart;
        [JsonProperty] public long DefaultMaxGold;
    }
}