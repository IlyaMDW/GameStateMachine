using System;
using Configs;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TriInspector;
using UnityEngine;

namespace Editor.Utils
{
    public class ConfigUtility : ScriptableObject
    {
        private enum Type
        {
            InfrastructureConfig,
            EnemyConfig,
            ResourceServiceConfig
        }

        [SerializeField] private InfrastructureConfig _infrastructureConfig;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private ResourceServiceConfig _resourceServiceConfig;

        [UsedImplicitly][SerializeField] private string _configName;
        [SerializeField] [PropertyOrder(12)] [TextArea(12, 9999)] private string _serialized;

        [Button] [PropertyOrder(10)]
        private void ToJson(Type type)
        {
            switch (type)
            {
                case Type.InfrastructureConfig:
                    _configName = ConfigType.InfrastructureConfig;
                    _serialized = JsonConvert.SerializeObject(_infrastructureConfig, Formatting.Indented);
                    break;
                case Type.EnemyConfig:
                    _configName = ConfigType.EnemyConfig;
                    _serialized = JsonConvert.SerializeObject(_enemyConfig, Formatting.Indented);
                    break;
                case Type.ResourceServiceConfig:
                    _configName = ConfigType.ResourceServiceConfig;
                    _serialized = JsonConvert.SerializeObject(_resourceServiceConfig, Formatting.Indented);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}