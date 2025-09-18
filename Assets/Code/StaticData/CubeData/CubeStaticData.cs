using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.StaticData.CubeData
{
    [CreateAssetMenu(menuName = "StaticData/Cube", fileName = "CubeConfig", order = 1)]
    public class CubeStaticData : ScriptableObject
    {
        [Header("Spawn Chances")]
        [SerializeField] public List<CubeSpawnChance> SpawnChances = new();
        [Header("Value Colors")]
        [SerializeField] public List<CubeValueColor> ValueColors = new();
        
        public Color GetColorForValue(int value)
        {
            CubeValueColor colorData = ValueColors.FirstOrDefault(x => x.Value == value);
            return colorData != null ? colorData.Color : Color.white;
        }
    }
}
