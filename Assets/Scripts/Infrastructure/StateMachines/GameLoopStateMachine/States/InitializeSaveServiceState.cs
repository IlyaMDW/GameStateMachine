using Infrastructure.Providers.PlayerDataProvider;
using Infrastructure.Services.Logging;
using Infrastructure.Services.ResourceService;
using Infrastructure.Services.Saving;
using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    [UsedImplicitly]
    public class InitializeSaveServiceState : BaseGameLoopState, IEnterableState
    {
        private readonly ISaveService _saveService;
        private readonly ResourceService _resourceService;
        private readonly PlayerDataProvider _playerDataProvider;
        private IConditionalLoggingService _conditionalLoggingService;

        public override string StateName => nameof(InitializeSaveServiceState);

        public InitializeSaveServiceState(
            GameLoopStateMachine gameLoopStateMachine,
            ISaveService saveService,
            ResourceService resourceService,
            PlayerDataProvider playerDataProvider,
            IConditionalLoggingService conditionalLoggingService
        ) : base(gameLoopStateMachine)
        {
            _conditionalLoggingService = conditionalLoggingService;
            _playerDataProvider = playerDataProvider;
            _resourceService = resourceService;
            _saveService = saveService;
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<AFKIncomeCalculationState>();
        }

        public void Enter()
        {
            InternalEnter();
        }

        private void InternalEnter(string saveName = null)
        {
            var tutorialCompleted = PlayerPrefs.GetInt("tutorialCompleted", 0);

            if (tutorialCompleted != 0)
            {
                _saveService.LoadSaveFile(saveName == null, saveName);
            }
            else
            {
                _conditionalLoggingService.Log("Skip loading save file in case unfinished tutorial", LogTag.SaveService);
            }
            
            _saveService.LoadAndAddToSave(_resourceService);
            _saveService.LoadAndAddToSave(_playerDataProvider);

            ToNextState();
        }

        public override void Exit()
        {
        }
    }
}