using Code.Logic.Cubes;
using Code.Services.Finish;
using UnityEngine;
using Zenject;

namespace Code.Logic.Triggers
{
    public class DefeatTrigger : MonoBehaviour
    {
        private IFinishService _finishService;
        
        [Inject]
        public void Construct(IFinishService finishService)
        {
            _finishService = finishService;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Cube>(out Cube cube))
                _finishService.Lose();
        }
    }
}
