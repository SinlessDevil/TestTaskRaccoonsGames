using System;
using Code.Logic.Cubes;

namespace Code.Services.CubeInput
{
    public interface ICubeInputService
    {
        event Action PushedCubeEvent;
        void Enable();
        void Disable();
        void SetCube(Cube cube);
    }
}