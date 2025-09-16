using Code.UI.Menu.ButtonsNavigation;
using Code.UI.Menu.Windows;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.Menu
{
    public class MenuHud : MonoBehaviour
    {
        [SerializeField] private ButtonNavigationHolder _buttonNavigationHolder;
        [SerializeField] private WindowHolder _windowHolder;
        [SerializeField] private ParallaxEffect _parallaxEffect;
        
        public void Initialize()
        {
            InitEventSystem();
            
            _parallaxEffect.Initialize();
            _buttonNavigationHolder.Initialize(TypeWindow.Map);
            _windowHolder.Initialize();
        }

        private void InitEventSystem()
        {
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem != null) 
                return;
            GameObject gameObjectEventSystem = new GameObject("EventSystem");
            gameObjectEventSystem.AddComponent<EventSystem>();
            gameObjectEventSystem.AddComponent<StandaloneInputModule>();

        }
    }
}