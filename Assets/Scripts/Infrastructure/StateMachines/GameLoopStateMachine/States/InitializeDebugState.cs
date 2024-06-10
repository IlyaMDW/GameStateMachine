﻿using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class InitializeDebugState : BaseGameLoopState, IEnterableState
    {
        private static bool IsInitialized { get; set; }

        private readonly AssetReferenceProvider _assetReferenceProvider;

        public override string StateName => nameof(InitializeDebugState);

        [Inject]
        public InitializeDebugState(GameLoopStateMachine stateMachine, AssetReferenceProvider assetReferenceProvider) : base(stateMachine)
        {
            _assetReferenceProvider = assetReferenceProvider;
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<InitializeDefaultConfigState>();
        }

        public async void Enter()
        {
            await InitializeDebugRoot();
        }

        private async UniTask InitializeDebugRoot()
        {
            if (IsInitialized)
            {
                ToNextState();
                return;
            }

            IsInitialized = true;
            await Task.CompletedTask;

#if DEV
            var debugRoot = await _assetReferenceProvider.DebugRootAssetReference.InstantiateAsync();
            Object.DontDestroyOnLoad(debugRoot);
#endif

            ToNextState();
        }
    }
}