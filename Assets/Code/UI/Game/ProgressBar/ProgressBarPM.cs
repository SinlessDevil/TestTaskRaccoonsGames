using System;
using System.Collections.Generic;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.StaticData.Levels;

namespace Code.UI.Game.ProgressBar
{
    public class ProgressBarPM
    {
        private readonly ILevelService _levelService;
        private readonly ILevelLocalProgressService _localProgressService;
        
        private LevelStaticData _currentLevelData;
        private List<int> _targetPoints = new List<int>();
        private int _currentMaxValue;
        private float _currentProgress;
        
        public ProgressBarPM(
            ILevelService levelService, 
            ILevelLocalProgressService localProgressService)
        {
            _levelService = levelService;
            _localProgressService = localProgressService;
        }
        
        public event Action<float> ProgressUpdatedEvent;
        public event Action<List<int>> TargetPointsUpdatedEvent;
        public event Action<int> CurrentValueUpdatedEvent;
        
        public List<int> TargetPoints => new(_targetPoints);
        public int HighestTarget { get; private set; }
        
        public void Initialize()
        {
            LoadLevelData();
            SubscribeToEvents();
            UpdateCurrentProgress();
            
            TargetPointsUpdatedEvent?.Invoke(_targetPoints);
            CurrentValueUpdatedEvent?.Invoke(_currentMaxValue);
            ProgressUpdatedEvent?.Invoke(_currentProgress);
        }
        
        public void Cleanup()
        {
            UnsubscribeFromEvents();
        }
        
        private void LoadLevelData()
        {
            _currentLevelData = _levelService.GetCurrentLevelStaticData();
            
            _targetPoints = new List<int>(_currentLevelData.TargetNumbers);
            _targetPoints.Sort();
            
            HighestTarget = _targetPoints.Count > 0 ? _targetPoints[_targetPoints.Count - 1] : 0;
        }
        
        private void SubscribeToEvents()
        {
            _localProgressService.MaxMergedNumberUpdatedEvent += OnMaxMergedNumberUpdated;
        }
        
        private void UnsubscribeFromEvents()
        {
            _localProgressService.MaxMergedNumberUpdatedEvent -= OnMaxMergedNumberUpdated;
        }
        
        private void OnMaxMergedNumberUpdated(int newMaxValue)
        {
            _currentMaxValue = newMaxValue;
            UpdateCurrentProgress();
            
            CurrentValueUpdatedEvent?.Invoke(_currentMaxValue);
            ProgressUpdatedEvent?.Invoke(_currentProgress);
        }
        
        private void UpdateCurrentProgress()
        {
            if (HighestTarget <= 0)
            {
                _currentProgress = 0f;
                return;
            }
            
            _currentProgress = UnityEngine.Mathf.Clamp01((float)_currentMaxValue / HighestTarget);
        }
        
        public float GetProgressForTarget(int targetValue)
        {
            if (HighestTarget <= 0) 
                return 0f;
            
            return UnityEngine.Mathf.Clamp01((float)targetValue / HighestTarget);
        }
        
        public bool IsTargetAchieved(int targetValue) => _currentMaxValue >= targetValue;
    }
}
