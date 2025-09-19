using Code.StaticData.Input;
using UnityEngine;

namespace Code.Services.Input.DeadZone
{
    public interface IInputDeadZoneService
    {
        bool CanTouch(Vector2 screenPoint);
        bool CanTouch(Vector3 screenPoint);
        bool IsPointInDeadZone(Vector2 screenPoint);
        bool IsPointInDeadZone(Vector3 screenPoint);
    }
}
