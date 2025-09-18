using Code.Logic.Cubes;
using Code.Services.CubeCoordinator;
using Code.Services.CubeInput;
using Code.Services.Input;
using Code.Services.Input.Device;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.Services.Providers;
using Code.Services.Timer;
using Code.UI;

namespace Code.Infrastructure.StateMachine.Game.States
{
    public class GameLoopState : IState, IGameState, IUpdatable
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IInputService _inputService;
        private readonly ILevelService _levelService;
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly ITimeService _timeService;
        private readonly ICubeInputService _cubeInputService;
        private readonly IPoolProvider<Widget> _widgetProvider;
        private readonly IPoolProvider<Cube> _cubeProvider;
        private readonly ICubeCoordinatorService _cubeCoordinatorService;

        public GameLoopState(
            IStateMachine<IGameState> gameStateMachine, 
            ILevelService levelService,
            ILevelLocalProgressService levelLocalProgressService,
            ITimeService timeService,
            IInputService inputService,
            ICubeInputService cubeInputService,
            IPoolProvider<Widget> widgetProvider,
            IPoolProvider<Cube> cubeProvider,
            ICubeCoordinatorService cubeCoordinatorService)
        {
            _gameStateMachine = gameStateMachine;
            _levelService = levelService;
            _levelLocalProgressService = levelLocalProgressService;
            _timeService = timeService;
            _inputService = inputService;
            _cubeInputService = cubeInputService;
            _widgetProvider = widgetProvider;
            _cubeProvider = cubeProvider;
            _cubeCoordinatorService = cubeCoordinatorService;
        }
        
        public void Enter()
        {
            _cubeCoordinatorService.Initialize();
            
            InitPools();

            InitInputs();
        }

        private void InitPools()
        {
            _widgetProvider.CreatePool();
            _cubeProvider.CreatePool();
        }
        
        private void InitInputs()
        {
            _inputService.SetInputDevice(new MouseInputDevice());
            _cubeInputService.Enable();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            _cubeCoordinatorService.Dispose();
            
            _widgetProvider.CleanupPool();
            _cubeProvider.CleanupPool();
            
            _cubeInputService.Cleanup();
            _inputService.Cleanup();
            
            _levelService.Cleanup();
            _levelLocalProgressService.Cleanup();
            
            _timeService.ResetTimer();
        }
    }
}