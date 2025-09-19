using Code.Services.StaticData;
using Code.StaticData.Input;
using UnityEngine;

namespace Code.Services.Input.DeadZone
{
    public class InputDeadZoneService : IInputDeadZoneService
    {
        private readonly IStaticDataService _staticDataService;
        
        public InputDeadZoneService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public bool CanTouch(Vector2 screenPoint) => 
            IsPointInWorkingArea(screenPoint);
        
        public bool CanTouch(Vector3 screenPoint) => 
            CanTouch(new Vector2(screenPoint.x, screenPoint.y));
        
        public bool IsPointInDeadZone(Vector2 screenPoint) => 
            !CanTouch(screenPoint);
        
        public bool IsPointInDeadZone(Vector3 screenPoint) => 
            !CanTouch(screenPoint);

        private bool IsPointInWorkingArea(Vector2 screenPoint)
        {
            Vector2 normalizedPoint = new Vector2(
                screenPoint.x / Screen.width,
                screenPoint.y / Screen.height
            );
            
            bool inWorkingArea = normalizedPoint.x >= DeadZoneConfig.WorkingAreaLeft && 
                                 normalizedPoint.x <= DeadZoneConfig.WorkingAreaRight &&
                                 normalizedPoint.y >= DeadZoneConfig.WorkingAreaBottom && 
                                 normalizedPoint.y <= DeadZoneConfig.WorkingAreaTop;
            
            return inWorkingArea;
        }
        
        private bool IsPointInWorkingArea(Vector3 screenPoint)
        {
            return IsPointInWorkingArea(new Vector2(screenPoint.x, screenPoint.y));
        }
        
        private DeadZoneStaticData DeadZoneConfig => _staticDataService.DeadZoneConfig;
    }
}
