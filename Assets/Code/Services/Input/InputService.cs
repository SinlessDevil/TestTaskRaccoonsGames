using System;
using Code.Services.Input.Device;
using UnityEngine;
using Zenject;

namespace Code.Services.Input
{
    public class InputService : IInputService, ITickable
    {
        private IInputDevice _inputDevice;
        
        public Vector2 Direction => _inputDevice?.Direction ?? Vector2.zero;
        public Vector3 TouchPositionToWorldPosition => _inputDevice?.TouchPositionToWorldPosition ?? Vector3.zero;
        public Vector3 TouchPosition => _inputDevice?.TouchPosition ?? Vector3.zero;
        public bool IsActiveInput => _inputDevice != null;

        public event Action<Vector2> TapEvent;
        public event Action PointerDownEvent;
        public event Action PointerUpEvent;
        public event Action InputUpdateEvent;

        private bool _isEnable;
        
        public void SetInputDevice(IInputDevice inputDevice)
        {
            if (_inputDevice != null)
            {
                UnsubscribeInputEvents(_inputDevice);
            }
            
            _inputDevice = inputDevice ?? throw new ArgumentNullException(nameof(inputDevice));
            
            SubscribeInputEvents(_inputDevice);
        }

        public void Tick()
        {
            if (_inputDevice == null || !_isEnable)
                return;

            _inputDevice.UpdateInput();
            InputUpdateEvent?.Invoke();
        }

        public void Cleanup()
        {
            SetInputDevice(new NullableInputDevice());
        }

        private void SubscribeInputEvents(IInputDevice inputDevice)
        {
            inputDevice.TapEvent += OnTap;
            inputDevice.PointerDownEvent += OnPointerDown;
            inputDevice.PointerUpEvent += OnPointerUp;
        }

        private void UnsubscribeInputEvents(IInputDevice inputDevice)
        {
            inputDevice.TapEvent -= OnTap;
            inputDevice.PointerDownEvent -= OnPointerDown;
            inputDevice.PointerUpEvent -= OnPointerUp;
        }

        private void OnTap(Vector2 position)
        {
            if(!_isEnable)
                return;
            
            TapEvent?.Invoke(position);
        }

        private void OnPointerDown()
        {
            if(!_isEnable)
                return;
            
            PointerDownEvent?.Invoke();
        }

        private void OnPointerUp()
        {
            if(!_isEnable)
                return;
            
            PointerUpEvent?.Invoke();
        }
    }
}
