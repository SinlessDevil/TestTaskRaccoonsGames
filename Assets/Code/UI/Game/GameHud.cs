using System.Collections.Generic;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.Services.StaticData;
using Code.UI.Game.ProgressBar;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.UI.Game
{
    public class GameHud : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private ProgressBarView _progressBarView;
        [Space(10)] 
        [SerializeField] private List<GameObject> _debugObjects;
        
        private ProgressBarPM _progressBarPM;
        
        private IStaticDataService _staticDataService;
        private ILevelService _levelService;
        private ILevelLocalProgressService _localProgressService;
        
        [Inject]
        public void Constructor(
            IStaticDataService staticDataService,
            ILevelService levelService,
            ILevelLocalProgressService localProgressService)
        {
            _staticDataService = staticDataService;
            _levelService = levelService;
            _localProgressService = localProgressService;
        }
        
        public void Initialize()
        {
            InitProgressBar();
            InitDebugObjects();
            TrySetUpEventSystem();
        }

        public void Cleanup()
        {
            CleanupProgressBar();
        }

        private void InitProgressBar()
        {
            _progressBarPM = new ProgressBarPM(_levelService, _localProgressService);
            _progressBarView.Initialize(_progressBarPM);
            _progressBarPM.Initialize();
        }
        
        private void CleanupProgressBar()
        {
            _progressBarPM.Cleanup();
            _progressBarPM = null;
            _progressBarView.Cleanup();
        }
        
        private void InitDebugObjects()
        {
            if (!_staticDataService.GameConfig.DebugMode) 
                return;
            
            foreach (var debugObject in _debugObjects) 
                debugObject.SetActive(true);
        }

        private static void TrySetUpEventSystem()
        {
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem != null) 
                return;
            
            GameObject gameObjectEventSystem = new GameObject("EventSystem");
            gameObjectEventSystem.AddComponent<EventSystem>();
            gameObjectEventSystem.AddComponent<StandaloneInputModule>();
        }
    }
}