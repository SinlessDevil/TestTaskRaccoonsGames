using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Game.ProgressBar
{
    public class PointView : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _fillImage;
        [SerializeField] private TextMeshProUGUI _numberText;
        [Header("Visual Settings")]
        [SerializeField] private Color _achievedColor = Color.green;
        [SerializeField] private Color _notAchievedColor = Color.gray;
        [SerializeField] private float _fillAnimationSpeed = 2f;
        
        private int _targetValue;
        private bool _isAchieved;
        private float _targetFillAmount;
        private bool _isAnimating;
        
        public void Initialize(int targetValue, bool isAchieved = false)
        {
            _targetValue = targetValue;
            _isAchieved = isAchieved;
            
            UpdateVisuals();
            UpdateText();
        }
        
        public void SetAchieved(bool achieved, bool animate = true)
        {
            if (_isAchieved == achieved) 
                return;
            
            _isAchieved = achieved;
            
            if (animate)
            {
                AnimateToAchieved();
            }
            else
            {
                UpdateVisuals();
            }
        }
        
        private void SetFillAmount(float fillAmount, bool animate = true)
        {
            _targetFillAmount = Mathf.Clamp01(fillAmount);
            
            if (animate)
            {
                _isAnimating = true;
            }
            else
            {
                if (_fillImage != null)
                    _fillImage.fillAmount = _targetFillAmount;
            }
        }
        
        private void Update()
        {
            if (_isAnimating && _fillImage != null)
            {
                float currentFill = _fillImage.fillAmount;
                float newFill = Mathf.MoveTowards(currentFill, _targetFillAmount, _fillAnimationSpeed * Time.deltaTime);
                
                _fillImage.fillAmount = newFill;
                
                if (Mathf.Approximately(newFill, _targetFillAmount))
                {
                    _isAnimating = false;
                }
            }
        }
        
        private void UpdateVisuals()
        {
            Color targetColor = _isAchieved ? _achievedColor : _notAchievedColor;
            
            _backgroundImage.color = targetColor;
            
            _fillImage.color = targetColor;
            _fillImage.fillAmount = _isAchieved ? 1f : 0f;
        }
        
        private void UpdateText() => 
            _numberText.text = _targetValue.ToString();

        private void AnimateToAchieved()
        {
            SetFillAmount(_isAchieved ? 1f : 0f, true);
            UpdateVisuals();
        }
        
        public int GetTargetValue() => _targetValue;
    }
}
