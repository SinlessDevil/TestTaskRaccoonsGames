using Code.Logic.Particles;
using Code.Services.Factories;
using UnityEngine;
using Zenject;

namespace Code.Services.Providers.Particles
{
    public sealed class CubeParticleFactory : Factory, IPoolFactory<ParticleHolder>
    {
        public CubeParticleFactory(IInstantiator instantiator) : base(instantiator)
        {
        }

        public ParticleHolder Create(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            ParticleHolder particle = Instantiate(ResourcePath.ExplosionParticlePath)
                .GetComponent<ParticleHolder>();
            
            particle.transform.position = position;
            particle.transform.rotation = rotation;
            
            if (parent != null)
                particle.transform.SetParent(parent);
            
            particle.gameObject.SetActive(false);
            
            return particle;
        }
    }
}
