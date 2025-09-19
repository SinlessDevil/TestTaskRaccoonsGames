using System;

namespace Code.Services.LocalProgress
{
    public class LevelLocalProgressService : ILevelLocalProgressService
    {
        public event Action<int> UpdateScoreEvent;
        public event Action<int> MaxMergedNumberUpdatedEvent;
        
        public int Score { get; private set; }
        public int MaxMergedNumber { get; private set; }
        
        public void AddScore(int score)
        {
            Score += score;
            UpdateScoreEvent?.Invoke(Score);
        }
        
        public void UpdateMaxMergedNumber(int mergedNumber)
        {
            if (mergedNumber > MaxMergedNumber)
            {
                MaxMergedNumber = mergedNumber;
                MaxMergedNumberUpdatedEvent?.Invoke(MaxMergedNumber);
            }
        }
        
        public void Cleanup()
        {
            Score = 0;
            MaxMergedNumber = 0;
        }
    }
}