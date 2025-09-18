using System;
using UnityEngine;

namespace Code.Logic.Cubes
{
    public class CubeColliderDetector : MonoBehaviour
    {
        public event Action<Cube> DetectedCubeEvent;
        
        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out Cube cube))
            {
                DetectedCubeEvent?.Invoke(cube);
            }
        }
    }
}