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
        [Header("Animation Settings")]
        [SerializeField] private float _arrowMoveSpeed = 3f;
        [SerializeField] private float _fillAnimationSpeed = 2f;
        [Header("Visual Settings")]
        [SerializeField] private Color _fillColor = Color.blue;
        [SerializeField] private float _arrowYOffset = 20f;
        
        private ProgressBarPM _progressBarPM;
        private List<PointView> _pointViews = new();
        
        private float _targetFillAmount;
        private float _targetArrowPosition;
        private bool _isAnimatingFill;
        private bool _isAnimatingArrow;
        
        public void Initialize(ProgressBarPM progressBarPM)
        {
            _progressBarPM = progressBarPM;
            
            SubscribeToEvents();
            SetupInitialVisuals();
            CreatePointViews();
            
            Debug.Log("[ProgressBarView] Initialized with PM");
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
            _targetFillAmount = progress;
            _isAnimatingFill = true;
            
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
            if (_arrowTransform == null) 
                return;
            
            RectTransform barRect = GetComponent<RectTransform>();
            float barWidth = barRect.rect.width;
            
            _targetArrowPosition = Mathf.Lerp(-barWidth * 0.5f, barWidth * 0.5f, progress);
            _isAnimatingArrow = true;
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
        
        private void Update()
        {
            AnimateFill();
            AnimateArrow();
        }
        
        private void AnimateFill()
        {
            if (_isAnimatingFill && _fillImage != null)
            {
                float currentFill = _fillImage.fillAmount;
                float newFill = Mathf.MoveTowards(currentFill, _targetFillAmount, _fillAnimationSpeed * Time.deltaTime);
                
                _fillImage.fillAmount = newFill;
                
                if (Mathf.Approximately(newFill, _targetFillAmount))
                {
                    _isAnimatingFill = false;
                }
            }
        }
        
        private void AnimateArrow()
        {
            if (_isAnimatingArrow && _arrowTransform != null)
            {
                Vector2 currentPos = _arrowTransform.anchoredPosition;
                float newX = Mathf.MoveTowards(currentPos.x, _targetArrowPosition, _arrowMoveSpeed * 100f * Time.deltaTime);
                
                _arrowTransform.anchoredPosition = new Vector2(newX, currentPos.y + _arrowYOffset);
                
                if (Mathf.Approximately(newX, _targetArrowPosition))
                {
                    _isAnimatingArrow = false;
                }
            }
        }
    }
}
