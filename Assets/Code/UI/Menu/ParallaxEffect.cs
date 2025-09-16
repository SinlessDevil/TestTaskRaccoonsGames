using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Menu
{
    public class ParallaxEffect : MonoBehaviour
    {
        [Header("Parallax Settings")]
        [SerializeField] private RectTransform _backgroundElement;
        [SerializeField] private float _parallaxSpeed = 1.0f;
        [SerializeField] private float _scrollSpeed = 100f;
        [SerializeField] private bool _autoScroll = true;
        [SerializeField] private bool _loopBackground = true;
        [SerializeField] private CanvasScaler _canvasScaler;
        [Header("Screen Size Response")]
        [SerializeField] private bool _respondToScreenSize = true;
        [SerializeField] private float _screenSizeMultiplier = 1f;
        [Header("Child Objects")]
        [SerializeField] private bool _includeChildObjects = true;
        [SerializeField] private float _childSpacing = 4260f;
        
        private Vector2 _initialPosition;
        private Vector2[] _childInitialPositions;
        private RectTransform[] _childObjects;
        private Vector2 _screenSize;
        private Vector2 _lastScreenSize;
        
        public void Initialize()
        {
            _initialPosition = _backgroundElement.anchoredPosition;
            if (_includeChildObjects) 
                InitializeChildObjects();
            UpdateScreenSize();
            _lastScreenSize = _screenSize;
        }
        
        private void InitializeChildObjects()
        {
            _childObjects = new RectTransform[_backgroundElement.childCount];
            _childInitialPositions = new Vector2[_backgroundElement.childCount];
            for (int i = 0; i < _backgroundElement.childCount; i++)
            {
                RectTransform child = _backgroundElement.GetChild(i) as RectTransform;
                if (child == null) 
                    continue;
                _childObjects[i] = child;
                _childInitialPositions[i] = child.anchoredPosition;
            }
        }
        
        private void Update()
        {
            if (_respondToScreenSize)
            {
                UpdateScreenSize();
                if (_screenSize != _lastScreenSize)
                {
                    OnScreenSizeChanged();
                    _lastScreenSize = _screenSize;
                }
            }
            
            ApplyParallaxEffect();
        }
        
        private void UpdateScreenSize() => 
            _screenSize = _canvasScaler != null ? _canvasScaler.referenceResolution : new Vector2(Screen.width, Screen.height);

        private void OnScreenSizeChanged()
        {
            if (_backgroundElement != null)
            {
                Vector2 newPosition = _initialPosition;
                newPosition.x *= _screenSizeMultiplier * (_screenSize.x / 1080f);
                _backgroundElement.anchoredPosition = newPosition;
            }
            
            if (_includeChildObjects && _childObjects != null)
            {
                for (int i = 0; i < _childObjects.Length; i++)
                {
                    if (_childObjects[i] == null || i >= _childInitialPositions.Length) 
                        continue;
                    
                    Vector2 newPosition = _childInitialPositions[i];
                    newPosition.x *= _screenSizeMultiplier * (_screenSize.x / 1080f);
                    _childObjects[i].anchoredPosition = newPosition;
                }
            }
        }
        
        private void ApplyParallaxEffect()
        {
            if (!_autoScroll)
                return;
            
            float deltaTime = Time.deltaTime;
            float movement = _scrollSpeed * _parallaxSpeed * deltaTime;
            Vector2 currentPos = _backgroundElement.anchoredPosition;
            Vector2 newPos = currentPos;
            newPos.x -= movement;
            
            if (_loopBackground)
                CheckAndResetSingleObject(newPos);
            else
                _backgroundElement.anchoredPosition = newPos;
        }
        
        private void CheckAndResetSingleObject(Vector2 newPos)
        {
            float objectSize = _childSpacing;
            
            if (newPos.x <= -objectSize)
            {
                Vector2 resetPos = newPos;
                resetPos.x = objectSize;
                _backgroundElement.anchoredPosition = resetPos;
                Debug.Log($"RESET: Position reset from {newPos.x:F2} to {objectSize}");
            }
            else
            {
                _backgroundElement.anchoredPosition = newPos;
            }
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_parallaxSpeed < 0)
                _parallaxSpeed = 0;
            
            if (_scrollSpeed < 0)
                _scrollSpeed = 0;
                
            if (_screenSizeMultiplier <= 0)
                _screenSizeMultiplier = 1f;
                
            if (_childSpacing < 0)
                _childSpacing = 0;
        }
        #endif
    }
}
