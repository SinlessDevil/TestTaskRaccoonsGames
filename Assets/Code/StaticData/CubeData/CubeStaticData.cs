using UnityEngine;

namespace Code.StaticData.CubeData
{
    [CreateAssetMenu(menuName = "StaticData/Cubes/Cube", fileName = "CubeConfig", order = 1)]
    public class CubeStaticData : ScriptableObject
    {
        [Header("Merge Physics")]
        [SerializeField] public float MergePushForce = 25f;
        [SerializeField] public Vector3 MergePushDirection = new Vector3(0, 0.7f, 0.7f);
        [SerializeField] public float MergeRandomForceMin = -20f;
        [SerializeField] public float MergeRandomForceMax = -10f;
        [SerializeField] public float MergeRandomForcePositiveMin = 10f;
        [SerializeField] public float MergeRandomForcePositiveMax = 20f;
        [SerializeField] public float MergeTorqueMin = -5f;
        [SerializeField] public float MergeTorqueMax = 5f;
        [Header("Input Control")]
        [SerializeField] public float InputLeftBoundary = -12.5f;
        [SerializeField] public float InputRightBoundary = 12f;
        [SerializeField] public float InputSmoothSpeed = 8f;
        [SerializeField] public float InputPushForce = 65f;
        [SerializeField] public Vector3 InputPushDirection = Vector3.forward;
    }
}
