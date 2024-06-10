using Infrastructure.Providers;
using Infrastructure.Services.Logging;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class FinalizeInitializationState : BaseGameLoopState, IEnterableState
    {
        private IConditionalLoggingService _conditionalLoggingService;
        public override string StateName => nameof(FinalizeInitializationState);

        public FinalizeInitializationState(GameLoopStateMachine stateMachine, IConditionalLoggingService conditionalLoggingService) : base(stateMachine)
        {
            _conditionalLoggingService = conditionalLoggingService;
        }

        private void ToNextState(SceneNames nextScene)
        {
            _gameLoopStateMachine.Enter<LoadSceneState, SceneNames>(nextScene);
        }

        public void Enter()
        {
            _conditionalLoggingService.Log($"Finish loading at: {Time.realtimeSinceStartup}", LogTag.GameLoopStateMachine);
#if UNITY_EDITOR
            ToNextState(InEditorConfig.Instance.SkipMenu ? SceneNames.Gameplay : SceneNames.Menu);
#else
            ToNextState(SceneNames.Gameplay);
#endif
        }

    }
}