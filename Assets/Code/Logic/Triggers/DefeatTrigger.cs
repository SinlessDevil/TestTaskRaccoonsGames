using System;
using Code.Logic.Cubes;
using UnityEngine;

namespace Code.Logic.Triggers
{
    public class DefeatTrigger : MonoBehaviour
    {
        public event Action LosedEvent;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Cube>(out Cube cube)) 
                LosedEvent?.Invoke();
        }
    }
}
