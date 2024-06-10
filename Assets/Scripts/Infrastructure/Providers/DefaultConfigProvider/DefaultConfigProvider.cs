using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using TriInspector;
using UnityEngine;
using Formatting = Newtonsoft.Json.Formatting;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Infrastructure.Providers.DefaultConfigProvider
{
    [Serializable]
    public class DefaultConfigProvider : IDefaultConfigProvider
    {
        [Serializable]
        public class StringContainer
        {
            [TextArea(12, 300)] public string String;
        }

        private const int TIME_OF_LOSS_OF_RELEVANCE_IN_MINUTES = 600;

        [SerializeField] private bool _overrideDevWithRemoteWhenFetch;
        [SerializeField] private bool _useDevConfigAsCachedAndDisableRemote;
        [HideIf(nameof(_useDevConfigAsCachedAndDisableRemote))] [SerializeField] private bool _isRemoteConfigEnabled;
        [SerializeField] [ReadOnly] private string _lastFetchDate;
        [SerializeField] [ReadOnly] [UsedImplicitly] private string _currentDate;
        [SerializeField] [ReadOnly] private int _minutesFromLastFetch;

        [SerializeField][ReadOnly] private StringContainer _cachedConfigString;
        [SerializeField] private StringContainer _devConfigString;

        private Dictionary<string, JToken> _cachedConfig;

        public IDictionary<string, JToken> CachedConfig
        {
            get
            {
#if DEV
                return _cachedConfig ??= JsonConvert.DeserializeObject<Dictionary<string, JToken>>(_useDevConfigAsCachedAndDisableRemote ? _devConfigString.String : _cachedConfigString.String);
#else
                return _cachedConfig ??= JsonConvert.DeserializeObject<Dictionary<string, JToken>>(_cachedConfigString.String);
#endif
            }
        }

        public void ClearCache()
        {
            _cachedConfig = null;
        }

        public bool IsRemoteConfigEnabled
        {
            get
            {
#if DEV
                return !_useDevConfigAsCachedAndDisableRemote && _isRemoteConfigEnabled;
#else
                return true;
#endif
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            _currentDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            var lastCachingDate = DateTime.Parse(_lastFetchDate);
            _minutesFromLastFetch = (int) (DateTime.Now - lastCachingDate).TotalMinutes;

            if (_minutesFromLastFetch < TIME_OF_LOSS_OF_RELEVANCE_IN_MINUTES)
            {
                FetchConfig();
            }
        }

        [Button] [PropertyOrder(10)]
        private void FetchConfig()
        {
            var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private async void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            await remoteConfig.ActivateAsync();
            Continuation();

            remoteConfig.App.Dispose();
            info.Dispose();
        }

        private void Continuation()
        {
            //var cache = FirebaseRemoteConfig.DefaultInstance.AllValues.ToDictionary(
            //    keyValuePair => keyValuePair.Key,
            //    keyValuePair => JToken.Parse(keyValuePair.Value.StringValue));

            Dictionary<string, JToken> cache = new Dictionary<string, JToken>();
            foreach (var item in FirebaseRemoteConfig.DefaultInstance.AllValues)
            {
                if (item.Key.Contains("config"))
                    cache[item.Key] = JToken.Parse(item.Value.StringValue);
                else
                    cache[item.Key] = item.Value.StringValue;
            }

            _cachedConfigString.String = JsonConvert.SerializeObject(cache, Formatting.Indented);

            _minutesFromLastFetch = 0;
            _lastFetchDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            if (!_overrideDevWithRemoteWhenFetch) return;

            _devConfigString.String = JsonConvert.SerializeObject(cache, Formatting.Indented);
        }
#endif
    }
}