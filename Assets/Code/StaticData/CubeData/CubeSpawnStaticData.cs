using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData.CubeData
{
    [CreateAssetMenu(menuName = "StaticData/Cubes/CubeSpawn", fileName = "CubeSpawnConfig", order = 3)]
    public class CubeSpawnStaticData : ScriptableObject
    {
        [Header("Spawn Chances")]
        [SerializeField] public List<CubeSpawnChance> SpawnChances = new();
        [Header("Spawn Timing")]
        [SerializeField] public float SpawnDelay = 1f;
        [SerializeField] public float InitialSpawnDelay = 0.5f;
        [Header("Spawn Position")]
        [SerializeField] public Vector3 SpawnPosition = new Vector3(0, 10, -5);
    }
}
