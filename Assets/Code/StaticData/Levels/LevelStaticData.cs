using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData.Levels
{
    [CreateAssetMenu(fileName = "LevelStaticData", menuName = "StaticData/Level", order = 0)]
    public class LevelStaticData : ScriptableObject
    {
        [Header("Level Info")]
        public string LevelName;
        public int LevelId;
        public LevelTypeId LevelTypeId;
        
        [Header("Victory Conditions")]
        [SerializeField] public List<int> TargetNumbers = new List<int>();
        
        public bool IsVictoryAchieved(int maxMergedNumber)
        {
            return TargetNumbers.Contains(maxMergedNumber);
        }
        
        public int GetHighestTarget()
        {
            return TargetNumbers.Count > 0 ? TargetNumbers[TargetNumbers.Count - 1] : 0;
        }
    }
}