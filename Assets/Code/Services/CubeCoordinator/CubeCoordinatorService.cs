using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Logic.Cubes;
using Code.Services.CubeInput;
using Code.Services.Providers;
using Code.Services.StaticData;
using Code.StaticData.CubeData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.CubeCoordinator
{
    public class CubeCoordinatorService : ICubeCoordinatorService
    {
        private readonly ICubeInputService _cubeInputService;
        private readonly IPoolProvider<Cube> _poolProvider;
        private readonly IStaticDataService _staticDataService;

        private Cube _currentCube;
        
        public CubeCoordinatorService(
            ICubeInputService cubeInputService,
            IPoolProvider<Cube> poolProvider,
            IStaticDataService staticDataService)
        {
            _cubeInputService = cubeInputService;
            _poolProvider = poolProvider;
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
            OnGetCubeAsync(0);
            
            _cubeInputService.PushedCubeEvent += OnGetCube;
        }

        public void Dispose()
        {
            _cubeInputService.PushedCubeEvent -= OnGetCube;
            
            _currentCube = null;
        }

        private void OnGetCube()
        {
            OnGetCubeAsync(300);
        }
        
        private async UniTask OnGetCubeAsync(int delay)
        {
            await Task.Delay(delay);
            
            Cube cube =_poolProvider.Get(Vector3.zero,Quaternion.identity, null);

            int value = GetRandomValue();
            Color color = GetCubeStaticData.GetColorForValue(value);
            
            cube.Initialize(value);
            cube.CubeView.Initialize(value, color);
            cube.CubeAnimator.PlaySpawn();
            
            _cubeInputService.SetCube(cube);
        }
        
        private int GetRandomValue()
        {
            List<CubeSpawnChance> spawnChances = GetCubeStaticData.SpawnChances;
            
            if (spawnChances == null || spawnChances.Count == 0)
            {
                Debug.LogWarning("No spawn chances configured!");
                return 2;
            }
            
            float totalChance = spawnChances.Sum(x => x.Chance);
            float randomValue = UnityEngine.Random.Range(0f, totalChance);
            float currentChance = 0f;
            
            foreach (var spawnChance in spawnChances)
            {
                currentChance += spawnChance.Chance;
                if (randomValue <= currentChance)
                    return spawnChance.Value;
            }
            
            return spawnChances[0].Value;
        }
        
        private CubeStaticData GetCubeStaticData => _staticDataService.CubeConfig;
    }
}
