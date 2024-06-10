using Infrastructure.Providers.DefaultConfigProvider;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class InitializeAnalyticsState : BaseGameLoopState, IEnterableState
    {
        private readonly IConditionalLoggingService _conditionalLoggingService;
        private readonly IAnalyticsLogService _analyticsLogService;
        private readonly IDefaultConfigProvider _defaultConfigProvider;

        public override string StateName => nameof(InitializeAnalyticsState);

        [Inject]
        public InitializeAnalyticsState(
            GameLoopStateMachine stateMachine,
            IConditionalLoggingService conditionalLoggingService,
            IAnalyticsLogService analyticsLogService,
            IDefaultConfigProvider defaultConfigProvider
        ) : base(stateMachine)
        {
            _defaultConfigProvider = defaultConfigProvider;
            _analyticsLogService = analyticsLogService;
            _conditionalLoggingService = conditionalLoggingService;
        }

        private void ToNextState()
        {
            if (_defaultConfigProvider.IsRemoteConfigEnabled)
            {
                _gameLoopStateMachine.Enter<InitializeRemoteConfigState>();
            }
            else
            {
                InitializeRemoteConfigState.ToNextState(_gameLoopStateMachine);
            }
        }
 
        public async void Enter()
        {
            await _analyticsLogService.Initialize();
            ToNextState();
        }
    }
}