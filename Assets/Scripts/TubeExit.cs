using System;
using UnityEngine;
using DG.Tweening;

public class TubeExit : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem exitEffect;
    private WobbleEffect wobbleEffect;

    private void Start()
    {
       wobbleEffect = GetComponent<WobbleEffect>();
    }

    public void PlayExitEffects(GameObject obj, Vector3 originalScale, float speed)
    {
        // Play exit particle effect
        if (exitEffect != null)
        {
            Instantiate(exitEffect, obj.transform.position, Quaternion.identity);
        }
        
        // Play wobble effect on exit tube
        if (wobbleEffect != null)
        {
            wobbleEffect.PlayWobbleAnimation();
        }
        
        // Grow at exit with a pop effect
        Sequence exitSequence = DOTween.Sequence();
        exitSequence.Append(obj.transform.DOScale(originalScale * 1.2f, 0.15f)
            .SetEase(Ease.OutQuad));
        exitSequence.Append(obj.transform.DOScale(originalScale, 0.15f)
            .SetEase(Ease.InOutQuad));
    }
}
