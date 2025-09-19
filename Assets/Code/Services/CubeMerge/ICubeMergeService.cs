using System;
using Code.Logic.Cubes;

namespace Code.Services.CubeMerge
{
    public interface ICubeMergeService
    {
        void MergeCubes(Cube currentCube, Cube targetCube);
        event Action<int> CubeMergedEvent;
    }
}
