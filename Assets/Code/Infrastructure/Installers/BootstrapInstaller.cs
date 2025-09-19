using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.Game;
using Code.Infrastructure.StateMachine.Game.States;
using Code.Logic.Cubes;
using Code.Services.AudioVibrationFX.Music;
using Code.Services.AudioVibrationFX.Sound;
using Code.Services.AudioVibrationFX.StaticData;
using Code.Services.AudioVibrationFX.Vibration;
using Code.Services.CubeCoordinator;
using Code.Services.CubeInput;
using Code.Services.CubeMerge;
using Code.Services.Factories.Game;
using Code.Services.Factories.UIFactory;
using Code.Services.Finish;
using Code.Services.Finish.Lose;
using Code.Services.Finish.Win;
using Code.Services.Input;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.Services.PersistenceProgress;
using Code.Services.Providers;
using Code.Services.Providers.Cubes;
using Code.Services.Providers.Widgets;
using Code.Services.Random;
using Code.Services.SaveLoad;
using Code.Services.StaticData;
using Code.Services.Timer;
using Code.Services.Window;
using Code.UI;
using UnityEngine;
using Zenject;
using Application = UnityEngine.Application;

namespace Code.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private LoadingCurtain _curtain;
        
        private RuntimePlatform Platform => Application.platform;

        public override void InstallBindings()
        {
            Debug.Log("Installer");

            BindMonoServices();
            BindServices();
            BindGameStateMachine();
            MakeInitializable();
        }
        
        public void Initialize() => BootstrapGame();

        private void BindMonoServices()
        {
            Container.Bind<ICoroutineRunner>().FromMethod(() => Container.InstantiatePrefabForComponent<ICoroutineRunner>(_coroutineRunner)).AsSingle();
            Container.Bind<ILoadingCurtain>().FromMethod(() => Container.InstantiatePrefabForComponent<ILoadingCurtain>(_curtain)).AsSingle();
            
            BindSceneLoader();
        }

        private void BindServices()
        {
            BindStaticDataService();
            BindFactories();
            BindInputsServices();
            
            Container.BindInterfacesTo<WindowService>().AsSingle();
            Container.BindInterfacesTo<RandomService>().AsSingle();
            Container.BindInterfacesTo<UnifiedSaveLoadFacade>().AsSingle();
            Container.BindInterfacesTo<LevelService>().AsSingle();
            Container.BindInterfacesTo<TimeService>().AsSingle();

            Container.BindInterfacesTo<CubeCoordinatorService>().AsSingle();
            Container.BindInterfacesTo<CubeMergeService>().AsSingle();
            
            BindDataServices();
            BindAudioVibrationService();
            BindFinishService();
        }

        private void BindFactories()
        {
            Container.BindInterfacesTo<UIFactory>().AsSingle();
            Container.BindInterfacesTo<GameFactory>().AsSingle();
            
            Container.Bind<IPoolFactory<Widget>>().To<WidgetFactory>().AsSingle();
            Container.Bind<IPoolProvider<Widget>>().To<WidgetProvider>().AsSingle();
            
            Container.Bind<IPoolFactory<Cube>>().To<CubeFactory>().AsSingle();
            Container.Bind<IPoolProvider<Cube>>().To<CubeProvider>().AsSingle();
        }

        private void BindInputsServices()
        {
            Container.BindInterfacesTo<InputService>().AsSingle();
            Container.BindInterfacesTo<CubeInputService>().AsSingle();
        }

        private void BindAudioVibrationService()
        {
            Container.BindInterfacesTo<SoundService>().AsSingle();
            Container.BindInterfacesTo<MusicService>().AsSingle();
            Container.BindInterfacesTo<VibrationService>().AsSingle();
            
            Container.Resolve<ISoundService>().Cache2DSounds();
            Container.Resolve<ISoundService>().CreateSoundsPool();
            
            Container.Resolve<IMusicService>().CacheMusic();
            Container.Resolve<IMusicService>().CreateMusicRoot();
        }
        
        private void BindDataServices()
        {
            Container.BindInterfacesTo<PersistenceProgressService>().AsSingle();
            Container.BindInterfacesTo<LevelLocalProgressService>().AsSingle();
        }

        private void BindFinishService()
        {
            Container.BindInterfacesTo<FinishService>().AsSingle();
            Container.BindInterfacesTo<WinService>().AsSingle();
            Container.BindInterfacesTo<LoseService>().AsSingle();
        }
        
        private void BindGameStateMachine()
        {
            Container.Bind<GameStateFactory>().AsSingle();
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            
            BindGameStates();
        }

        private void MakeInitializable() => Container.Bind<IInitializable>().FromInstance(this);

        private void BindSceneLoader()
        {
            ISceneLoader sceneLoader = new SceneLoader(Container.Resolve<ICoroutineRunner>());
            Container.Bind<ISceneLoader>().FromInstance(sceneLoader).AsSingle();
        }

        private void BindStaticDataService()
        {
            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.LoadData();
            Container.Bind<IStaticDataService>().FromInstance(staticDataService).AsSingle();
            
            IAudioVibrationStaticDataService audioVibrationStaticDataService = new AudioVibrationStaticDataService();
            audioVibrationStaticDataService.LoadData();
            Container.Bind<IAudioVibrationStaticDataService>().FromInstance(audioVibrationStaticDataService).AsSingle();
        }
        
        private void BindGameStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadProgressState>().AsSingle();
            Container.Bind<BootstrapAnalyticState>().AsSingle();
            Container.Bind<PreLoadGameState>().AsSingle();
            Container.Bind<LoadMenuState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
        }

        private void BootstrapGame() => Container.Resolve<IStateMachine<IGameState>>().Enter<BootstrapState>();
    }
}