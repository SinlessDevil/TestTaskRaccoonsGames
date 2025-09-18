using Code.Logic.Cubes;
using Code.Services.CubeInput;
using Code.Services.CubeManager;
using Code.Services.Providers;
using UnityEngine;

namespace Code.Services.CubeCoordinator
{
    public class CubeCoordinatorService : ICubeCoordinatorService
    {
        private readonly ICubeInputService _cubeInputService;
        private readonly IPoolProvider<Cube> _poolProvider;
        
        private Cube _currentCube;
        
        public CubeCoordinatorService(
            ICubeInputService cubeInputService,
            IPoolProvider<Cube> poolProvider)
        {
            _cubeInputService = cubeInputService;
            _poolProvider = poolProvider;
        }

        public void Initialize()
        {
            OnGetCube();
            
            _cubeInputService.PushedCubeEvent += OnGetCube;
        }

        public void Dispose()
        {
            _cubeInputService.PushedCubeEvent -= OnGetCube;
            
            _currentCube = null;
        }

        private void OnGetCube()
        {
            Cube cube =_poolProvider.Get(Vector3.zero,Quaternion.identity, null);
            cube.CubeAnimator.PlaySpawn();
            _cubeInputService.SetCube(cube);
        }
    }
}
