using UnityEngine;

namespace Code.StaticData.Input
{
    [CreateAssetMenu(menuName = "StaticData/Input/DeadZone", fileName = "DeadZoneConfig", order = 0)]
    public class DeadZoneStaticData : ScriptableObject
    {
        [Header("Working Area (Normalized 0-1)")]
        [Tooltip("Left boundary of working area")]
        [Range(0f, 1f)] public float WorkingAreaLeft = 0f;
        [Tooltip("Right boundary of working area")]
        [Range(0f, 1f)] public float WorkingAreaRight = 1f;
        [Tooltip("Bottom boundary of working area")]
        [Range(0f, 1f)] public float WorkingAreaBottom = 0f;
        [Tooltip("Top boundary of working area")]
        [Range(0f, 1f)] public float WorkingAreaTop = 0.85f;
        [Header("Debug")]
        [SerializeField] private bool _showDebugInfo = false;
    }
}
