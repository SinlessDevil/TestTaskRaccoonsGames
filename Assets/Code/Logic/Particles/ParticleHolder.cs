using System.Threading.Tasks;
using Code.Services.Providers;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Code.Logic.Particles
{
    public class ParticleHolder : MonoBehaviour
    {
        [Header("Particle System")]
        [SerializeField] private ParticleSystem _mergeParticleSystem;
        [Header("Settings")]
        [SerializeField] private int _autoReturnDelay = 2;
        
        private IPoolProvider<ParticleHolder> _particlePoolProvider;
        
        [Inject]
        public void Constructor(IPoolProvider<ParticleHolder> particlePoolProvider)
        {
            _particlePoolProvider = particlePoolProvider;
        }
        
        public void PlayMerge(Color color)
        {
            SetParticleColor(color);
            gameObject.SetActive(true);
            _mergeParticleSystem.Play();
            ReturnToPool().Forget();
        }
        
        private void SetParticleColor(Color color)
        {
            ParticleSystem.MainModule main = _mergeParticleSystem.main;
            main.startColor = color;
        }
        
        private async UniTask ReturnToPool()
        {
            await Task.Delay(_autoReturnDelay * (1000));
            _particlePoolProvider.Return(this);
        }
        
        public void SetPosition(Vector3 position) => 
            transform.position = position;
    }
}
