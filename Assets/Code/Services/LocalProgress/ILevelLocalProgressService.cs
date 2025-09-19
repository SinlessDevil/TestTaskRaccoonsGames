using System;

namespace Code.Services.LocalProgress
{
    public interface ILevelLocalProgressService
    {
        void AddScore(int score);
        void UpdateMaxMergedNumber(int mergedNumber);
        
        int Score { get; }
        int MaxMergedNumber { get; }
        
        event Action<int> UpdateScoreEvent;
        event Action<int> MaxMergedNumberUpdatedEvent;
        
        void Cleanup();
    }   
}