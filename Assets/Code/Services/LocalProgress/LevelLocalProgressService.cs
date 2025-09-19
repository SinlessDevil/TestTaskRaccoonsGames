using System;

namespace Code.Services.LocalProgress
{
    public class LevelLocalProgressService : ILevelLocalProgressService
    {
        public event Action<int> MaxMergedNumberUpdatedEvent;
        
        public int MaxMergedNumber { get; private set; }
        
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
            MaxMergedNumber = 0;
        }
    }
}