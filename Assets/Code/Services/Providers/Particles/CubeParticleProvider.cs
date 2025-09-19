using Code.Logic.Particles;
using UnityEngine;

namespace Code.Services.Providers.Particles
{
    public sealed class CubeParticleProvider : BasePoolProvider<ParticleHolder>
    {
        private const int CountPool = 5;

        private Transform _root;
        
        public CubeParticleProvider(IPoolFactory<ParticleHolder> factory) : base(factory) { }
        
        public override void CreatePool()
        {
            _root = CreateRoot();
            CreatePool(CountPool, _root);
        }

        public override void CleanupPool()
        {
            base.CleanupPool();
            Object.Destroy(_root.gameObject);
        }
        
        public override ParticleHolder Get(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            ParticleHolder particle = base.Get(position, rotation, parent);
            particle.SetPosition(position);
            return particle;
        }
        
        public override void Return(ParticleHolder particle)
        {
            particle.gameObject.SetActive(false);
            base.Return(particle);
        }
        
        private Transform CreateRoot()
        {
            GameObject root = new GameObject(typeof(CubeParticleProvider).Name);
            return root.transform;
        }
    }
}
