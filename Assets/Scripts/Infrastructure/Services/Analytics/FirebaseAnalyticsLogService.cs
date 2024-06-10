using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Infrastructure.Services.Logging;
using UnityEngine.Device;
using Zenject;

namespace Infrastructure.Services.Analytics
{
    public class FirebaseAnalyticsLogService : IAnalyticsLogService
    {
        private readonly IConditionalLoggingService _conditionalLoggingService;

        [Inject]
        public FirebaseAnalyticsLogService(IConditionalLoggingService conditionalLoggingService)
        {
            _conditionalLoggingService = conditionalLoggingService;
        }

        public async UniTask Initialize()
        {
            await ResolveDependenciesAndInitialize();
        }

        public void LogEvent(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);

            _conditionalLoggingService.Log($"{eventName} sent", LogTag.Analytics);
        }

        private async UniTask ResolveDependenciesAndInitialize()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                _conditionalLoggingService.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus} ", LogTag.Analytics);
            }
        }

        private void InitializeFirebase()
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");
            

#if UNITY_EDITOR || DEVELOPMENT_BUILD || DEV
            FirebaseAnalytics.SetUserProperty("DEV", true.ToString(CultureInfo.InvariantCulture));
#else
            FirebaseAnalytics.SetUserProperty("DEV", false.ToString(CultureInfo.InvariantCulture));
#endif
            
#if UNITY_EDITOR
            FirebaseAnalytics.SetUserId($"DEVELOPER_{Environment.UserName}");
#elif DEV
            FirebaseAnalytics.SetUserId($"TESTER_{UnityEngine.Device.SystemInfo.deviceUniqueIdentifier}");
#else
            FirebaseAnalytics.SetUserId($"USER_{UnityEngine.Device.SystemInfo.deviceUniqueIdentifier}");
#endif
            FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));

            _conditionalLoggingService.Log("Firebase Analytics initialized", LogTag.Analytics);

            LogEvent(FirebaseAnalytics.EventLogin);
#if DEV
            LogEvent("SetDEV");
#endif
        }
    }
}