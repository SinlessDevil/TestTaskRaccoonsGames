using Code.Services.Input;
using UnityEngine;

namespace Code.Services.CubeInput
{
    public class CubeInputService : ICubeInputService
    {
        private Cube _cube;
        
        private float _leftPosition = -2f;
        private float _rightPosition = 2f;
        private float _smoothSpeed = 8f;
        
        private Vector2 _startTouchPosition;
        private Vector3 _targetPosition;
        private Vector3 _initialCubePosition;
            
        private bool _isPressed;
        
        private readonly IInputService _inputService;
        
        public CubeInputService(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        public void Enable()
        {
            SubscribeToInputEvents();
        }
        
        public void Disable()
        {
            UnsubscribeFromInputEvents();
        }
        
        public void SetupCube(Cube cube)
        {
            _cube = cube;
            _targetPosition = _cube.transform.position;
        }
        
        public void SetBoundaries(float leftPosition, float rightPosition)
        {
            _leftPosition = leftPosition;
            _rightPosition = rightPosition;
        }
        
        public void SetSmoothSpeed(float smoothSpeed)
        {
            _smoothSpeed = Mathf.Max(0.1f, smoothSpeed);
        }
        
        private void SubscribeToInputEvents()
        {
            _inputService.PointerDownEvent += OnPointerDown;
            _inputService.PointerUpEvent += OnPointerUp;
            _inputService.InputUpdateEvent += Tick;
        }
        
        private void UnsubscribeFromInputEvents()
        {
            _inputService.PointerDownEvent -= OnPointerDown;
            _inputService.PointerUpEvent -= OnPointerUp;
            _inputService.InputUpdateEvent -= Tick;
        }
        
        public void Tick()
        {
            if (_isPressed)
            {
                UpdateTargetPosition();
            }
            
            SmoothMoveCube();
        }
        
        private void OnPointerDown()
        {
            _isPressed = true;
            _startTouchPosition = GetNormalizedTouchPosition();
            
            _initialCubePosition = _cube.transform.position;
            _targetPosition = _initialCubePosition;
            
            Debug.Log($"Swipe started at: {_startTouchPosition}");
        }
        
        private void OnPointerUp()
        {
            _isPressed = false;
            Debug.Log("Swipe ended");
        }
        
        private Vector2 GetNormalizedTouchPosition()
        {
            Vector3 touchPosition = _inputService.TouchPosition;
            float normalizedX = (touchPosition.x / Screen.width) - 0.5f;
            return new Vector2(normalizedX, 0);
        }
        
        private void UpdateTargetPosition()
        {
            Vector2 currentTouchPosition = GetNormalizedTouchPosition();
            Vector2 swipeDelta = currentTouchPosition - _startTouchPosition;
            float swipeWorldDistance = swipeDelta.x * (_rightPosition - _leftPosition);
            float newWorldX = _initialCubePosition.x + swipeWorldDistance;
            
            newWorldX = Mathf.Clamp(newWorldX, _leftPosition, _rightPosition);
            _targetPosition = new Vector3(newWorldX, _initialCubePosition.y, _initialCubePosition.z);
        }
        
        private void SmoothMoveCube()
        {
            Vector3 currentPosition = _cube.transform.position;
            Vector3 newPosition = Vector3.Lerp(currentPosition, _targetPosition, _smoothSpeed * Time.deltaTime);
            _cube.transform.position = newPosition;
        }
    }
}
