using Code.Logic.Triggers;

namespace Code.Services.Finish
{
    public interface IFinishService
    {
        void Initialize(DefeatTrigger[] defeatTriggers);
        void Cleanup();
        public void Win();
        public void Lose();
    }   
}