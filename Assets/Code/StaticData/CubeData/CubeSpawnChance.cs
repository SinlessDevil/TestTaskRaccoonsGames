using System;
using UnityEngine;

namespace Code.StaticData.CubeData
{
    [Serializable]
    public class CubeSpawnChance
    {
        public int Value = 2;
        [Range(0f, 100f)] public float Chance = 50f;
    }
}