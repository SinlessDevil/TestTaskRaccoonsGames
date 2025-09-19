using System;
using Code.Logic.Cubes;
using Code.Services.AudioVibrationFX.Sound;
using Code.Services.AudioVibrationFX.Vibration;
using Code.Services.Factories.UIFactory;
using Code.Services.Input;
using Code.Services.StaticData;
using Code.StaticData.CubeData;
using Code.UI.Game;
using UnityEngine;

namespace Code.Services.CubeInput
{
    public class CubeInputService : ICubeInputService
    {
        private Cube _cube;
        
        private Vector2 _startTouchPosition;
        private Vector3 _targetPosition;
        private Vector3 _initialCubePosition;
        
        private bool _isFirstTick = true;
        private bool _isPressed;
        
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticDataService;
        private readonly ISoundService _soundService;
        private readonly IVibrationService _vibrationService;
        private readonly IUIFactory _uiFactory;

        public CubeInputService(
            IInputService inputService, 
            IStaticDataService staticDataService,
            ISoundService soundService,
            IVibrationService vibrationService,
            IUIFactory uiFactory)
        {
            _inputService = inputService;
            _staticDataService = staticDataService;
            _soundService = soundService;
            _vibrationService = vibrationService;
            _uiFactory = uiFactory;
        }

        public event Action PushedCubeEvent;

        public void Enable()
        {
            SubscribeToInputEvents();
        }
        
        public void Disable()
        {
            UnsubscribeFromInputEvents();
        }

        public void SetCube(Cube cube)
        {
            _cube = cube;
        }
        
        private void SubscribeToInputEvents()
        {
            _inputService.PointerDownEvent += OnPointerDown;
            _inputService.PointerUpEvent += OnPointerUp;
            _inputService.InputUpdateEvent += OnUpdateInput;
        }
        
        private void UnsubscribeFromInputEvents()
        {
            _inputService.PointerDownEvent -= OnPointerDown;
            _inputService.PointerUpEvent -= OnPointerUp;
            _inputService.InputUpdateEvent -= OnUpdateInput;
        }
        
        private void OnUpdateInput()
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
            
            _soundService.PlaySound(Sound2DType.Push);
            _vibrationService.Play(VibrationType.SuccessPreset);
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
            float swipeWorldDistance = swipeDelta.x * (CubeStaticData.InputRightBoundary - CubeStaticData.InputLeftBoundary);
            float newWorldX = _initialCubePosition.x + swipeWorldDistance;
            
            newWorldX = Mathf.Clamp(newWorldX, CubeStaticData.InputLeftBoundary, CubeStaticData.InputRightBoundary);
            _targetPosition = new Vector3(newWorldX, _initialCubePosition.y, _initialCubePosition.z);
        }
        
        private void SmoothMoveCube()
        {
            Vector3 currentPosition = _cube.transform.position;
            Vector3 newPosition = Vector3.Lerp(currentPosition, _targetPosition, CubeStaticData.InputSmoothSpeed * Time.deltaTime);
            _cube.transform.position = newPosition;
        }
        
        private void PushCube()
        {
            Vector3 pushVector = CubeStaticData.InputPushForce * CubeStaticData.InputPushDirection;
            _cube.Rigidbody.AddForce(pushVector, ForceMode.Impulse);
        }

        private Vector2 GetNormalizedTouchPosition()
        {
            Vector3 touchPosition = _inputService.TouchPosition;
            float normalizedX = (touchPosition.x / Screen.width) - 0.5f;
            return new Vector2(normalizedX, 0);
        }

        private CubeStaticData CubeStaticData => _staticDataService.CubeStaticData;
    }
}