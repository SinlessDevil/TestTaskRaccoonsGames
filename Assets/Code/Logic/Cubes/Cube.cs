using System;
using UnityEngine;

namespace Code.Logic.Cubes
{
    public class Cube : MonoBehaviour
    {
        [Header("Cube Components")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CubeAnimator _cubeAnimator;
        [SerializeField] private CubeColliderDetector _colliderDetector;

        public void OnValidate()
        {
            if (_rigidbody == null)
                _rigidbody = GetComponentInChildren<Rigidbody>();
            
            if(_cubeAnimator == null)
                _cubeAnimator = GetComponentInChildren<CubeAnimator>();
        }

        public Rigidbody Rigidbody => _rigidbody;
        public CubeAnimator CubeAnimator => _cubeAnimator;

        public void Enable()
        {
            gameObject.SetActive(true);
            Rigidbody.isKinematic = false;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            Rigidbody.isKinematic = true;
        }
    }

    public class CubeColliderDetector : MonoBehaviour
    {
        public void OnCollisionEnter(Collision other)
        {
            
        }
    }
}