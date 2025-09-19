using System;

namespace Code.Services.LocalProgress
{
    public interface ILevelLocalProgressService
    {
        event Action<int> MaxMergedNumberUpdatedEvent;
        int MaxMergedNumber { get; }
        void UpdateMaxMergedNumber(int mergedNumber);
        void Cleanup();
    }   
}