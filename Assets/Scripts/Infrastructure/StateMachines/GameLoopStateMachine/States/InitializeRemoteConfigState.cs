using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Configs;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.StateMachine;
using Newtonsoft.Json.Linq;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class InitializeRemoteConfigState : BaseGameLoopState, IEnterableState
    {
        private readonly IConditionalLoggingService _conditionalLoggingService;

        public override string StateName => nameof(InitializeRemoteConfigState);

        [Inject]
        public InitializeRemoteConfigState(
            GameLoopStateMachine stateMachine,
            IConditionalLoggingService conditionalLoggingService) : base(stateMachine)
        {
            _conditionalLoggingService = conditionalLoggingService;
        }

        public static void ToNextState(GameLoopStateMachine stateMachine)
        {
            stateMachine.Enter<InitializeSaveServiceState>();
        }

        public async void Enter()
        {
            await InitializeRemoteSettings();
        }

        private async UniTask InitializeRemoteSettings()
        {
            await FetchDataAsync();
        }

        public Task FetchDataAsync()
        {
            var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                _conditionalLoggingService.LogError("Retrieval hasn't finished.", LogTag.RemoteSettings);
                return;
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                _conditionalLoggingService.LogError(
                    $"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}",
                    LogTag.RemoteSettings);
                return;
            }

            remoteConfig.ActivateAsync()
                .ContinueWithOnMainThread(
                    _ =>
                    {
                        _conditionalLoggingService.Log(
                            "Remote data loaded.",
                            LogTag.RemoteSettings);

                        //var processedDictionary = FirebaseRemoteConfig.DefaultInstance.AllValues.ToDictionary(
                        //    keyValuePair => keyValuePair.Key,
                        //    keyValuePair => JToken.Parse(keyValuePair.Value.StringValue)
                        //);
                        Dictionary<string, JToken> processedDictionary = new Dictionary<string, JToken>();
                        foreach (var item in FirebaseRemoteConfig.DefaultInstance.AllValues)
                        {
                            if (item.Key.Contains("config"))
                                processedDictionary[item.Key] = JToken.Parse(item.Value.StringValue);
                            else
                                processedDictionary[item.Key] = item.Value.StringValue;
                        }

                        Remote.InitializeByRemote(processedDictionary, _conditionalLoggingService);
                        ToNextState(_gameLoopStateMachine);
                    });
        }
    }
}