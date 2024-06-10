using System;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class EntryPointState : BaseGameLoopState, IEnterableState
    {
        private IConditionalLoggingService _conditionalLoggingService;
        public override string StateName => nameof(EntryPointState);

        [Inject]
        public EntryPointState(GameLoopStateMachine stateMachine, IConditionalLoggingService conditionalLoggingService) : base(stateMachine)
        {
            _conditionalLoggingService = conditionalLoggingService;
        }

        private void ToNextState()
        {
            _conditionalLoggingService.Log($"Start loading at: {Time.realtimeSinceStartup}", LogTag.GameLoopStateMachine);
            _gameLoopStateMachine.Enter<LoadSceneState, Action>(OnLoadingSceneLoadedCallback);
        }

        private void OnLoadingSceneLoadedCallback()
        {
            _gameLoopStateMachine.Enter<InitializeDebugState>();
        }

        public void Enter()
        {
            ToNextState();
        }
    }
}