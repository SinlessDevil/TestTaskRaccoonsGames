using System.Threading.Tasks;
using Code.Services.Window;
using Code.Window;
using Code.Window.Finish.Lose;
using Cysharp.Threading.Tasks;

namespace Code.Services.Finish.Lose
{
    public class LoseService : ILoseService
    {
        private const int DelayToShowWindow = 500;
        
        private IWindowService _windowService;

        public LoseService(
            IWindowService windowService)
        {
            _windowService = windowService;
        }
        
        public void Lose()
        {
            ShowWindow().Forget();
        }
        
        private async UniTask ShowWindow()
        {
            await Task.Delay(DelayToShowWindow);
            
            var window = _windowService.Open(WindowTypeId.Lose);
            var loseWindow = window.GetComponent<LoseWindow>();
            loseWindow.Initialize();
        }
    }
}