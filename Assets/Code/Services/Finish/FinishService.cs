using Code.Services.CubeMerge;
using Code.Services.Finish.Lose;
using Code.Services.Finish.Win;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.StaticData.Levels;

namespace Code.Services.Finish
{
    public class FinishService : IFinishService
    {
        private readonly IWinService _winService;
        private readonly ILoseService _loseService;
        private readonly ILevelService _levelService;
        private readonly ICubeMergeService _cubeMergeService;
        private readonly ILevelLocalProgressService _levelLocalProgressService;

        public FinishService(
            IWinService winService, 
            ILoseService loseService,
            ILevelService levelService,
            ICubeMergeService cubeMergeService,
            ILevelLocalProgressService levelLocalProgressService)
        {
            _winService = winService;
            _loseService = loseService;
            _levelService = levelService;
            _cubeMergeService = cubeMergeService;
            _levelLocalProgressService = levelLocalProgressService;
        }

        public void Initialize()
        {
            _cubeMergeService.CubeMergedEvent += OnCubeMerged;
            _levelLocalProgressService.MaxMergedNumberUpdatedEvent += OnMaxMergedNumberUpdated;
        }
        
        public void Cleanup()
        {
            _cubeMergeService.CubeMergedEvent -= OnCubeMerged;
            _levelLocalProgressService.MaxMergedNumberUpdatedEvent -= OnMaxMergedNumberUpdated;
        }
        
        public void Win()
        {
            switch (_levelService.GetCurrentLevelStaticData().LevelTypeId)
            {
                case LevelTypeId.Regular:
                    _winService.Win();
                    break;
            }
        }

        public void Lose()
        {
            _loseService.Lose();
        }
        
        private void OnCubeMerged(int newMergedValue)
        {
            _levelLocalProgressService.UpdateMaxMergedNumber(newMergedValue);
        }
        
        private void OnMaxMergedNumberUpdated(int maxMergedNumber)
        {
            if (LevelStaticData.IsVictoryAchieved(maxMergedNumber))
            {
                Win();
            }
        }
        
        private LevelStaticData LevelStaticData => _levelService.GetCurrentLevelStaticData();
    }
}
