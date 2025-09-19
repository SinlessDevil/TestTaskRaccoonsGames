using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Game.ProgressBar
{
    public class ProgressBarView : MonoBehaviour
    {
        [Header("Progress Bar Components")]
        [SerializeField] private Image _fillImage;
        [SerializeField] private RectTransform _arrowTransform;
        [SerializeField] private Transform _pointsContainer;
        [Header("Point Prefab")]
        [SerializeField] private PointView _pointPrefab;
        [Header("Visual Settings")]
        [SerializeField] private Color _fillColor = Color.blue;
        [SerializeField] private Vector2 _arrowOffset;
        
        private ProgressBarPM _progressBarPM;
        private List<PointView> _pointViews = new();
        
        public void Initialize(ProgressBarPM progressBarPM)
        {
            _progressBarPM = progressBarPM;
            
            SubscribeToEvents();
            SetupInitialVisuals();
            CreatePointViews();
        }
        
        public void Cleanup()
        {
            UnsubscribeFromEvents();
            ClearPointViews();
        }
        
        private void SubscribeToEvents()
        {
            _progressBarPM.ProgressUpdatedEvent += OnProgressUpdated;
            _progressBarPM.TargetPointsUpdatedEvent += OnTargetPointsUpdated;
            _progressBarPM.CurrentValueUpdatedEvent += OnCurrentValueUpdated;
        }
        
        private void UnsubscribeFromEvents()
        {
            _progressBarPM.ProgressUpdatedEvent -= OnProgressUpdated;
            _progressBarPM.TargetPointsUpdatedEvent -= OnTargetPointsUpdated;
            _progressBarPM.CurrentValueUpdatedEvent -= OnCurrentValueUpdated;
        }
        
        private void SetupInitialVisuals()
        {
            _fillImage.color = _fillColor;
            _fillImage.fillAmount = 0f;
        }
        
        private void CreatePointViews()
        {
            ClearPointViews();
            
            List<int> targetPoints = _progressBarPM.TargetPoints;
            
            for (int i = 0; i < targetPoints.Count; i++)
            {
                int targetValue = targetPoints[i];
                
                PointView pointView = Instantiate(_pointPrefab, _pointsContainer);
                pointView.Initialize(targetValue, _progressBarPM.IsTargetAchieved(targetValue));
                float progress = _progressBarPM.GetProgressForTarget(targetValue);
                PositionPoint(pointView.GetComponent<RectTransform>(), progress);
                
                _pointViews.Add(pointView);
            }
        }
        
        private void ClearPointViews()
        {
            foreach (var pointView in _pointViews)
            {
                if (pointView != null)
                    DestroyImmediate(pointView.gameObject);
            }
            _pointViews.Clear();
        }
        
        private void PositionPoint(RectTransform pointRect, float progress)
        {
            Vector2 anchoredPosition = pointRect.anchoredPosition;
            anchoredPosition.x = Mathf.Lerp(-GetComponent<RectTransform>().rect.width * 0.5f, 
                                           GetComponent<RectTransform>().rect.width * 0.5f, 
                                           progress);
            pointRect.anchoredPosition = anchoredPosition;
        }
        
        private void OnProgressUpdated(float progress)
        {
            _fillImage.fillAmount = progress;
            UpdateArrowPosition(progress);
        }
        
        private void OnTargetPointsUpdated(List<int> targetPoints)
        {
            CreatePointViews();
        }
        
        private void OnCurrentValueUpdated(int currentValue)
        {
            UpdatePointsAchievement();
        }
        
        private void UpdateArrowPosition(float progress)
        {
            RectTransform barRect = GetComponent<RectTransform>();
            float barWidth = barRect.rect.width;
            
            float targetX = Mathf.Lerp(-barWidth * 0.5f, barWidth * 0.5f, progress);
            Vector2 finalPosition = new Vector2(targetX, 0f) + _arrowOffset;
            
            _arrowTransform.anchoredPosition = finalPosition;
        }
        
        private void UpdatePointsAchievement()
        {
            foreach (var pointView in _pointViews)
            {
                if (pointView != null)
                {
                    int targetValue = pointView.GetTargetValue();
                    bool shouldBeAchieved = _progressBarPM.IsTargetAchieved(targetValue);
                    pointView.SetAchieved(shouldBeAchieved, true);
                }
            }
        }
        
    }
}
