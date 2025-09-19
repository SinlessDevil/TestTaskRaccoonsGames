using System;
using Code.Logic.Cubes;
using Code.Services.Input;
using Code.Services.StaticData;
using UnityEngine;

namespace Code.Services.CubeInput
{
    public class CubeInputService : ICubeInputService
    {
        private Cube _cube;
        
        private float _leftPosition;
        private float _rightPosition;
        private float _smoothSpeed;
        private float _pushForce;
        private Vector3 _pushDirection;
        
        private Vector2 _startTouchPosition;
        private Vector3 _targetPosition;
        private Vector3 _initialCubePosition;
        
        private bool _isFirstTick = true;
        private bool _isPressed;
        
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticDataService;
        
        public CubeInputService(IInputService inputService, IStaticDataService staticDataService)
        {
            _inputService = inputService;
            _staticDataService = staticDataService;
            
            InitializeFromStaticData();
        }
        
        private void InitializeFromStaticData()
        {
            var cubeConfig = _staticDataService.CubeStaticData;
            _leftPosition = cubeConfig.InputLeftBoundary;
            _rightPosition = cubeConfig.InputRightBoundary;
            _smoothSpeed = cubeConfig.InputSmoothSpeed;
            _pushForce = cubeConfig.InputPushForce;
            _pushDirection = cubeConfig.InputPushDirection;
        }

        public event Action PushedCubeEvent;

        public void Enable()
        {
            SubscribeToInputEvents();
        }
        
        public void Cleanup()
        {
            UnsubscribeFromInputEvents();
        }

        public void SetCube(Cube cube)
        {
            _cube = cube;
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
                SmoothMoveCube();
            }
        }
        
        private void OnPointerDown()
        {
            _isPressed = true;
            _isFirstTick = true;
            
            _initialCubePosition = _cube.transform.position;
            _targetPosition = _initialCubePosition;
        }
        
        private void OnPointerUp()
        {
            _isPressed = false;
            
            PushCube();
            
            PushedCubeEvent?.Invoke();
        }
        
        private void UpdateTargetPosition()
        {
            Vector2 currentTouchPosition = GetNormalizedTouchPosition();
            
            if (_isFirstTick)
            {
                _startTouchPosition = currentTouchPosition;
                _isFirstTick = false;
                return;
            }
            
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
        
        private void PushCube()
        {
            Vector3 pushVector = _pushDirection * _pushForce;
            _cube.Rigidbody.AddForce(pushVector, ForceMode.Impulse);
        }

        private Vector2 GetNormalizedTouchPosition()
        {
            Vector3 touchPosition = _inputService.TouchPosition;
            float normalizedX = (touchPosition.x / Screen.width) - 0.5f;
            return new Vector2(normalizedX, 0);
        }
    }
}
