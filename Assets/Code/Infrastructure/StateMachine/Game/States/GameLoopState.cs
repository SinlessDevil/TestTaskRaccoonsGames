using Code.Logic.Cubes;
using Code.Logic.Particles;
using Code.Services.CubeCoordinator;
using Code.Services.CubeInput;
using Code.Services.Finish;
using Code.Services.Input;
using Code.Services.Input.Device;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.Services.Providers;
using Code.Services.Timer;
using Code.Services.Finish;

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
        private readonly IPoolProvider<Cube> _cubeProvider;
        private readonly IPoolProvider<ParticleHolder> _particleHolderProvider;
        private readonly ICubeCoordinatorService _cubeCoordinatorService;
        private readonly IFinishService _finishService;

        public GameLoopState(
            IStateMachine<IGameState> gameStateMachine, 
            ILevelService levelService,
            ILevelLocalProgressService levelLocalProgressService,
            ITimeService timeService,
            IInputService inputService,
            ICubeInputService cubeInputService,
            IPoolProvider<Cube> cubeProvider,
            IPoolProvider<ParticleHolder> particleHolderProvider,
            ICubeCoordinatorService cubeCoordinatorService,
            IFinishService finishService)
        {
            _gameStateMachine = gameStateMachine;
            _levelService = levelService;
            _levelLocalProgressService = levelLocalProgressService;
            _timeService = timeService;
            _inputService = inputService;
            _cubeInputService = cubeInputService;
            _cubeProvider = cubeProvider;
            _particleHolderProvider = particleHolderProvider;
            _cubeCoordinatorService = cubeCoordinatorService;
            _finishService = finishService;
        }
        
        public void Enter()
        {
            _cubeCoordinatorService.Initialize();
            _finishService.Initialize();
            
            InitPools();

            InitInputs();
        }

        private void InitPools()
        {
            _particleHolderProvider.CreatePool();
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
            _finishService.Cleanup();
            
            _particleHolderProvider.CleanupPool();
            _cubeProvider.CleanupPool();
            
            _cubeInputService.Disable();
            _inputService.Cleanup();
            
            _levelService.Cleanup();
            _levelLocalProgressService.Cleanup();
            
            _timeService.ResetTimer();
        }
    }
}