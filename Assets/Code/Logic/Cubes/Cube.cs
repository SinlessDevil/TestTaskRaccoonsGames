using UnityEngine;
using Zenject;

namespace Code.Logic.Cubes
{
    public class Cube : MonoBehaviour
    {
        [Header("Cube Components")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CubeAnimator _cubeAnimator;
        [SerializeField] private CubeColliderDetector _cubeColliderDetector;
        [SerializeField] private CubeView _cubeView;

        public void OnValidate()
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
            
            if(_cubeAnimator == null)
                _cubeAnimator = GetComponent<CubeAnimator>();
            
            if(_cubeColliderDetector == null)
                _cubeColliderDetector = GetComponent<CubeColliderDetector>();
            
            if(_cubeView == null)
                _cubeView = GetComponent<CubeView>();
        }
        
        [Inject]
        public void Constructor()
        {
            
        }

        public Rigidbody Rigidbody => _rigidbody;
        public CubeAnimator CubeAnimator => _cubeAnimator;
        public CubeView CubeView => _cubeView;
        public int Value { get; private set; }

        public void Initialize(int value)
        {
            Value = value;
        }
        
        public void Enable()
        {
            gameObject.SetActive(true);
            
            Rigidbody.isKinematic = false;

            _cubeColliderDetector.DetectedCubeEvent += OnDetectedCubeEvent;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            
            Rigidbody.isKinematic = true;

            _cubeColliderDetector.DetectedCubeEvent -= OnDetectedCubeEvent;
        }

        private void OnDetectedCubeEvent(Cube targetCube)
        {
            
        }
    }
}