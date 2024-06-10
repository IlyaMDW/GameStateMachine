using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Configs;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.PlayerDataProvider;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using Infrastructure.Services.ResourceService;
using Infrastructure.Services.Saving;
using Infrastructure.StateMachines.StateMachine;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Utils.Extensions;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class IncomeContainer
    {
        public string MoneyEarnedText { get; set; }
        public long MoneyEarned { get; set; }
    }

    public class AFKIncomeCalculationState : BaseGameLoopState, IEnterableState, IDataSaveable<AFKIncomeCalculationState.Save>
    {
        public static IncomeContainer IncomeContainer { get; private set; }

        private ISaveService _saveService;
        private ResourceService _resourceService;
        private IPlayerDataProvider _playerDataProvider;

        private float _timer;
        private float _updateDateTime = 3;
        private ICoroutineRunnerService _coroutineRunnerService;
        private IConditionalLoggingService _conditionalLoggingService;

        public class Save
        {
            public DateTime lastInDate;
            public long lastInGold;
            public long lastInEnergy;
        }

        public override string StateName => nameof(AFKIncomeCalculationState);

        public AFKIncomeCalculationState(GameLoopStateMachine stateMachine,
            ISaveService saveService,
            ResourceService resourceService,
            IPlayerDataProvider playerDataProvider,
            ICoroutineRunnerService coroutineRunnerService,
            IConditionalLoggingService conditionalLoggingService) : base(stateMachine)
        {
            _conditionalLoggingService = conditionalLoggingService;
            _coroutineRunnerService = coroutineRunnerService;
            _playerDataProvider = playerDataProvider;
            _resourceService = resourceService;
            _saveService = saveService;
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<FinalizeInitializationState>();
        }

        public void Enter()
        {
            InternalEnter();
        }

        private async void InternalEnter()
        {
            _saveService.LoadAndAddToSave(this);
            
            CalculateAFKIncome((float) (await GetServerTime() - SaveData.lastInDate).TotalSeconds);
            
            _coroutineRunnerService.StartCoroutine(UpdateLastInDateCoroutine());
            
            ResourceServiceOnOnAnyUpdated();
            _resourceService.OnAnyUpdated += ResourceServiceOnOnAnyUpdated;
            
            GC.Collect();

            ToNextState();
        }

        private void CalculateAFKIncome(float totalSeconds)
        {   

        }

        private IEnumerator UpdateLastInDateCoroutine()
        {
            while (true)
            {
                if (_timer > _updateDateTime)
                {
                    _timer -= _updateDateTime;
                    UpdateLastInDate();
                }

                _timer += Time.deltaTime;
                yield return null;
            }
        }

        private void ResourceServiceOnOnAnyUpdated()
        {
            SaveData.lastInGold = _resourceService.Gold;
        }

        private async Task UpdateLastInDate()
        {
            SaveData.lastInDate = await GetServerTime();
        }

        private async UniTask<DateTime> GetServerTime()
        {
            var client = new HttpClient();
            var responseBody = await client.GetStringAsync("http://worldtimeapi.org/api/timezone/Europe/Moscow");
            var response = JObject.Parse(responseBody);
            return DateTime.Parse(response["datetime"].ToString());
        }

        public string SaveId => SaveKeys.CalculateAFKIncomeState;

        public Save SaveData { get; set; }

        public Save Default => new Save()
        {
            lastInDate = DateTime.Now
        };
    }
}