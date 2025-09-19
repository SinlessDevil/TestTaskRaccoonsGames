using Code.Logic.Cubes;
using Code.Logic.Particles;
using Code.Services.Providers;
using Code.Services.StaticData;
using Code.StaticData.CubeData;
using UnityEngine;

namespace Code.Services.CubeMerge
{
    public class CubeMergeService : ICubeMergeService
    {
        private readonly IPoolProvider<Cube> _cubePoolProvider;
        private readonly IPoolProvider<ParticleHolder> _particlePoolProvider;
        private readonly IStaticDataService _staticDataService;

        public CubeMergeService(
            IPoolProvider<Cube> cubePoolProvider,
            IPoolProvider<ParticleHolder> particlePoolProvider,
            IStaticDataService staticDataService)
        {
            _cubePoolProvider = cubePoolProvider;
            _particlePoolProvider = particlePoolProvider;
            _staticDataService = staticDataService;
        }

        public void MergeCubes(Cube currentCube, Cube targetCube)
        {
            if (currentCube.Value != targetCube.Value)
                return;
            
            int newValue = currentCube.Value * 2;
            Vector3 mergePosition = CalculateMergePosition(currentCube, targetCube);
            
            _cubePoolProvider.Return(currentCube);
            _cubePoolProvider.Return(targetCube);
            
            Cube newCube = _cubePoolProvider.Get(mergePosition, Quaternion.identity, null);
            Color newColor = _staticDataService.CubeColorStaticData.GetColorForValue(newValue);
            newCube.Initialize(newValue);
            newCube.CubeView.Initialize(newValue, newColor);
            
            ApplyMergePhysics(newCube);
            
            PlayExplosionEffect(mergePosition,newColor);
        }

        private Vector3 CalculateMergePosition(Cube currentCube, Cube targetCube)
        {
            Vector3 currentPos = currentCube.transform.position;
            Vector3 targetPos = targetCube.transform.position;
            return Vector3.Lerp(currentPos, targetPos, 0.5f);
        }

        private void ApplyMergePhysics(Cube newCube)
        {
            newCube.Rigidbody.velocity = Vector3.zero;
            newCube.Rigidbody.angularVelocity = Vector3.zero;
            
            newCube.Rigidbody.AddForce(CubeStaticData.MergePushDirection * CubeStaticData.MergePushForce, ForceMode.Impulse);
            
            float randomValue = UnityEngine.Random.Range(0f, 1f) > 0.5f 
                ? UnityEngine.Random.Range(CubeStaticData.MergeRandomForceMin, CubeStaticData.MergeRandomForceMax) 
                : UnityEngine.Random.Range(CubeStaticData.MergeRandomForcePositiveMin, CubeStaticData.MergeRandomForcePositiveMax);
            Vector3 randomDirection = Vector3.one * randomValue;
            newCube.Rigidbody.AddForce(randomDirection);
            
            Vector3 randomTorque = new Vector3(
                UnityEngine.Random.Range(CubeStaticData.MergeTorqueMin, CubeStaticData.MergeTorqueMax),
                UnityEngine.Random.Range(CubeStaticData.MergeTorqueMin, CubeStaticData.MergeTorqueMax),
                UnityEngine.Random.Range(CubeStaticData.MergeTorqueMin, CubeStaticData.MergeTorqueMax)
            );
            newCube.Rigidbody.AddTorque(randomTorque, ForceMode.Impulse);
        }

        private void PlayExplosionEffect(Vector3 position, Color color)
        {
            ParticleHolder explosionParticle = _particlePoolProvider.Get(position, Quaternion.identity);
            
            // Option 1: Simple color
            explosionParticle.PlayMerge(color);
            
            // Option 2: Color with custom alpha (commented out)
            // explosionParticle.SetParticleColorWithAlpha(color, 0.8f);
            // explosionParticle.gameObject.SetActive(true);
            // explosionParticle._mergeParticleSystem.Play();
        }

        private CubeStaticData CubeStaticData => _staticDataService.CubeStaticData;
    }
}
