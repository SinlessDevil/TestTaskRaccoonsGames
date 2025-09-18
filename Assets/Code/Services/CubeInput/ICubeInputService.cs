using System;
using Code.Logic.Cubes;

namespace Code.Services.CubeInput
{
    public interface ICubeInputService
    {
        event Action PushedCubeEvent;
        void Enable();
        void Cleanup();
        void SetCube(Cube cube);
    }
}
