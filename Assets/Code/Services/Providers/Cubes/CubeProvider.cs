using Code.Logic.Cubes;
using UnityEngine;

namespace Code.Services.Providers.Cubes
{
    public sealed class CubeProvider : BasePoolProvider<Cube>
    {
        private const int CountPool = 10;

        private Transform _root;
        
        public CubeProvider(IPoolFactory<Cube> factory) : base(factory) { }
        
        public override void CreatePool()
        {
            _root = CreatRoot();
            CreatePool(CountPool, _root);
        }

        public override void CleanupPool()
        {
            base.CleanupPool();
            Object.Destroy(_root.gameObject);
        }
        
        public override Cube Get(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            Cube cube = base.Get(position, rotation, parent);
            cube.Enable();
            return cube;
        }
        
        public override void Return(Cube cube)
        {
            if (cube != null)
            {
                cube.Disable();
            }
            base.Return(cube);
        }
        
        private Transform CreatRoot()
        {
            GameObject root = new GameObject(typeof(CubeProvider).Name);
            return root.transform;
        }
    }
}