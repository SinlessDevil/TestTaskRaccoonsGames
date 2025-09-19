using Code.Logic.Triggers;
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

        private DefeatTrigger[] _defeatTriggers;
        private bool _isFinishing;
        
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

        public void Initialize(DefeatTrigger[] defeatTriggers)
        {
            _defeatTriggers = defeatTriggers;
            foreach (DefeatTrigger defeatTrigger in _defeatTriggers) 
                defeatTrigger.LosedEvent += Lose;
            
            _cubeMergeService.CubeMergedEvent += OnCubeMerged;
            _levelLocalProgressService.MaxMergedNumberUpdatedEvent += OnMaxMergedNumberUpdated;
        }
        
        public void Cleanup()
        {
            _isFinishing = false;
            
            foreach (DefeatTrigger defeatTrigger in _defeatTriggers) 
                defeatTrigger.LosedEvent -= Lose;
            _defeatTriggers = null;
            
            _cubeMergeService.CubeMergedEvent -= OnCubeMerged;
            _levelLocalProgressService.MaxMergedNumberUpdatedEvent -= OnMaxMergedNumberUpdated;
        }
        
        public void Win()
        {
            if(_isFinishing)
                return;
            _isFinishing = true;
            
            switch (_levelService.GetCurrentLevelStaticData().LevelTypeId)
            {
                case LevelTypeId.Regular:
                    _winService.Win();
                    break;
            }
        }

        public void Lose()
        {
            if(_isFinishing)
                return;
            _isFinishing = true;
            
            _loseService.Lose();
        }
        
        private void OnCubeMerged(int newMergedValue)
        {
            _levelLocalProgressService.UpdateMaxMergedNumber(newMergedValue);
        }
        
        private void OnMaxMergedNumberUpdated(int maxMergedNumber)
        {
            if (LevelStaticData.IsVictoryAchieved(maxMergedNumber)) 
                Win();
        }
        
        private LevelStaticData LevelStaticData => _levelService.GetCurrentLevelStaticData();
    }
}
