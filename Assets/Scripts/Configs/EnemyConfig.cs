using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Configs
{
    [Serializable][JsonObject]
    public class EnemyConfigData
    {
        [JsonProperty] public int MinHp;
        [JsonProperty] public int MaxHp;
        [JsonProperty] public int MinKillGold;
        [JsonProperty] public int MaxKillGold;
        [JsonProperty] public int Exp = 500;
        [JsonProperty] public float SpawnChance = 0.3f;
    }

    [Serializable][JsonObject]
    public class EnemyConfigListContainer
    {
        [JsonProperty] public List<EnemyConfigData> DataForAct;
    }

    [Serializable][JsonObject]
    public class EnemyConfig : IConfig
    {
        [JsonProperty] public EnemyConfigData TutorialEnemy;
        [JsonProperty] public List<EnemyConfigListContainer> EnemyConfigData;
    }
}