using Code.Logic.Cubes;
using Code.Services.Providers;
using Code.Services.StaticData;
using Code.StaticData.CubeData;
using UnityEngine;

namespace Code.Services.CubeMerge
{
    public class CubeMergeService : ICubeMergeService
    {
        private readonly IPoolProvider<Cube> _cubePoolProvider;
        private readonly IStaticDataService _staticDataService;

        public CubeMergeService(
            IPoolProvider<Cube> cubePoolProvider,
            IStaticDataService staticDataService)
        {
            _cubePoolProvider = cubePoolProvider;
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
            Color newColor = GetCubeStaticData.GetColorForValue(newValue);
            newCube.Initialize(newValue);
            newCube.CubeView.Initialize(newValue, newColor);
            
            ApplyMergePhysics(newCube);
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
            
            float pushForce = 25f;
            newCube.Rigidbody.AddForce(new Vector3(0, 0.7f, 0.7f) * pushForce, ForceMode.Impulse);
            
            float randomValue = UnityEngine.Random.Range(0f, 1f) > 0.5f ? UnityEngine.Random.Range(-20f, -10f) : 
                UnityEngine.Random.Range(10f, 20f);
            Vector3 randomDirection = Vector3.one * randomValue;
            newCube.Rigidbody.AddForce(randomDirection);
            
            Vector3 randomTorque = new Vector3(
                UnityEngine.Random.Range(-5f, 5f),
                UnityEngine.Random.Range(-5f, 5f),
                UnityEngine.Random.Range(-5f, 5f)
            );
            newCube.Rigidbody.AddTorque(randomTorque, ForceMode.Impulse);
        }

        private CubeStaticData GetCubeStaticData => _staticDataService.CubeConfig;
    }
}
