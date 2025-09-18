using Code.Services.CubeInput;
using Code.Services.Input;
using Code.Services.Input.Device;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.Services.Providers.Widgets;
using Code.Services.Timer;
using UnityEngine;

namespace Code.Infrastructure.StateMachine.Game.States
{
    public class GameLoopState : IState, IGameState, IUpdatable
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IInputService _inputService;
        private readonly IWidgetProvider _widgetProvider;
        private readonly ILevelService _levelService;
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly ITimeService _timeService;
        private readonly ICubeInputService _cubeInputService;

        public GameLoopState(
            IStateMachine<IGameState> gameStateMachine, 
            IInputService inputService,
            IWidgetProvider widgetProvider,
            ILevelService levelService,
            ILevelLocalProgressService levelLocalProgressService,
            ITimeService timeService,
            ICubeInputService cubeInputService)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _widgetProvider = widgetProvider;
            _levelService = levelService;
            _levelLocalProgressService = levelLocalProgressService;
            _timeService = timeService;
            _cubeInputService = cubeInputService;
        }
        
        public void Enter()
        {
            _inputService.SetInputDevice(new MouseInputDevice());
            
            var cube = Object.FindAnyObjectByType<Cube>();
            _cubeInputService.SetupCube(cube);
            _cubeInputService.SetBoundaries(-12.5f,12.5f);
            _cubeInputService.Enable();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            _cubeInputService.Disable();
            
            _inputService.Cleanup();
            _widgetProvider.CleanupPool();
            _levelService.Cleanup();
            _levelLocalProgressService.Cleanup();
            
            _timeService.ResetTimer();
        }
    }
}