using UnityEngine;
using Zenject;

namespace Code.Logic.Triggers
{
    public class SceneTriggersInstaller : MonoInstaller
    {
        [Header("Scene Objects")]
        [SerializeField] private DefeatTrigger[] _defeatTriggers;
        
        public override void InstallBindings()
        {
            foreach (var trigger in _defeatTriggers)
                Container.QueueForInject(trigger);
        }
    }
}
