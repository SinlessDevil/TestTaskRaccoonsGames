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
            Color newColor = _staticDataService.CubeColorStaticData.GetColorForValue(newValue);
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

        private CubeStaticData CubeStaticData => _staticDataService.CubeStaticData;
    }
}
