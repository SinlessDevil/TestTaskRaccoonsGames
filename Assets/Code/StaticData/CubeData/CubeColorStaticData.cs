using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.StaticData.CubeData
{
    [CreateAssetMenu(menuName = "StaticData/Cubes/CubeColor", fileName = "CubeColorConfig", order = 2)]
    public class CubeColorStaticData : ScriptableObject
    {
        [Header("Value Colors")]
        [SerializeField] public List<CubeValueColor> ValueColors = new();
        
        public Color GetColorForValue(int value)
        {
            CubeValueColor colorData = ValueColors.FirstOrDefault(x => x.Value == value);
            return colorData != null ? colorData.Color : Color.white;
        }
    }
}
