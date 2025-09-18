using Code.Logic.Cubes;
using Code.Services.Factories;
using UnityEngine;
using Zenject;

namespace Code.Services.Providers.Cubes
{
    public sealed class CubeFactory : Factory, IPoolFactory<Cube>
    {
        
        public CubeFactory(IInstantiator instantiator) : base(instantiator)
        {
            
        }

        public Cube Create(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            Cube cube = Instantiate(ResourcePath.CubePath)
                .GetComponent<Cube>();
            
            cube.transform.position = position;
            cube.transform.rotation = rotation;
            
            cube.Disable();
            
            return cube;
        }
    }
}