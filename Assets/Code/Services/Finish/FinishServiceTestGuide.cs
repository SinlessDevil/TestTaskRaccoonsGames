using UnityEngine;

namespace Code.Services.Finish
{
    /// <summary>
    /// Test guide for the updated FinishService with victory/defeat logic
    /// This class provides information on how to test the system
    /// </summary>
    public class FinishServiceTestGuide : MonoBehaviour
    {
        [Header("Testing Instructions")]
        [TextArea(10, 20)]
        [SerializeField] private string _testingInstructions = @"
TESTING THE FINISH SERVICE:

1. SETUP LEVEL DATA:
   - Open LevelStaticData ScriptableObject
   - Set Target Numbers (e.g., [16, 32, 64])
   
2. VICTORY TESTING:
   - Merge cubes to reach target numbers
   - When maxMergedNumber matches any target → Win() called
   - Check console for win messages
   
3. DEFEAT TESTING:
   - Create DefeatTrigger in scene
   - Add BoxCollider with isTrigger = true
   - Position below game area
   - When cube touches trigger → Lose() called
   
4. FLOW VERIFICATION:
   CubeMergeService.CubeMergedEvent 
   → FinishService.OnCubeMerged 
   → LevelLocalProgressService.UpdateMaxMergedNumber
   → FinishService.OnMaxMergedNumberUpdated
   → Check LevelStaticData.IsVictoryAchieved
   → If true: FinishService.Win()
   
5. DEFEAT FLOW:
   DefeatTrigger.OnTriggerEnter
   → FinishService.Lose()
   
6. DEBUG LOGS:
   - Enable debug mode in components
   - Check console for event flow
   - Verify max merged number updates
        ";
        
        private void OnValidate()
        {
            // This component is for documentation only
            if (Application.isPlaying)
            {
                Debug.Log("[FinishServiceTestGuide] This component is for testing guidance only.");
            }
        }
    }
}
