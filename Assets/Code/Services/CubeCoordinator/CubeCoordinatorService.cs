using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Code.Logic.Cubes;
using Code.Services.CubeInput;
using Code.Services.Providers;
using Code.Services.StaticData;
using Code.StaticData.CubeData;
using UnityEngine;
using Cysharp.Threading.Tasks;

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
            OnGetCubeAsync((int)(SpawnConfig.InitialSpawnDelay * 1000));
            
            _cubeInputService.PushedCubeEvent += OnGetCube;
        }

        public void Dispose()
        {
            _cubeInputService.PushedCubeEvent -= OnGetCube;
            
            _currentCube = null;
        }

        private void OnGetCube()
        {
            OnGetCubeAsync((int)(SpawnConfig.SpawnDelay * 1000));
        }
        
        private async UniTask OnGetCubeAsync(int delay)
        {
            await Task.Delay(delay);
            
            Cube cube = _poolProvider.Get(SpawnConfig.SpawnPosition, Quaternion.identity, null);

            int value = GetRandomValue();
            Color color = _staticDataService.CubeColorStaticData.GetColorForValue(value);
            
            cube.Initialize(value);
            cube.CubeView.Initialize(value, color);
            cube.CubeAnimator.PlaySpawn();
            
            _cubeInputService.SetCube(cube);
        }
        
        private int GetRandomValue()
        {
            List<CubeSpawnChance> spawnChances = _staticDataService.CubeSpawnStaticData.SpawnChances;
            
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
        
        private CubeSpawnStaticData SpawnConfig => _staticDataService.CubeSpawnStaticData;
    }
}
