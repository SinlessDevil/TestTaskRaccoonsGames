using DG.Tweening;
using UnityEngine;

namespace Code.Logic.Cubes
{
    public class CubeAnimator : MonoBehaviour
    {
        public void PlaySpawn()
        {
            transform.localScale = Vector3.zero;
            
            transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.InBounce);
        }
    }
}