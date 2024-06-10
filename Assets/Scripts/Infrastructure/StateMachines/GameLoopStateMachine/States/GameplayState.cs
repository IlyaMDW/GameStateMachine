using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Saving;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.MonoBehaviours;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class GameplayState : BaseGameLoopState, IEnterableState
    {
        private readonly ISaveService _saveService;
        private readonly ICoroutineRunnerService _coroutineRunnerService;

        private Coroutine _autoSaveCoroutine;
        public override string StateName => nameof(GameplayState);

        [Inject]
        public GameplayState(GameLoopStateMachine gameLoopStateMachine, ISaveService saveService, ICoroutineRunnerService coroutineRunnerService) : base(gameLoopStateMachine)
        {
            _coroutineRunnerService = coroutineRunnerService;
            _saveService = saveService;
        }

        //State changes by GameStateSwitchButton in scene
        public void Enter()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            UnityCallbacksRetranslator.ApplicationFocus -= OnApplicationFocus;
            UnityCallbacksRetranslator.ApplicationFocus += OnApplicationFocus;
        }

        public override void Exit()
        {
            base.Exit();
            UnityCallbacksRetranslator.ApplicationFocus -= OnApplicationFocus;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) return;

            _saveService.StoreSaveFile(false);
        }

        private void OnSceneUnloaded(Scene _)
        {
            _saveService.StoreSaveFile(false);
        }
    }
}