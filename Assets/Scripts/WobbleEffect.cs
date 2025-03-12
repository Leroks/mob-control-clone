using UnityEngine;
using DG.Tweening;

public class WobbleEffect : MonoBehaviour
{
    [Header("Wobble Settings")]
    [SerializeField] private float wobbleStrength = 1.2f;
    [SerializeField] private float wobbleDuration = 0.3f;
    [SerializeField] private int wobbleVibrato = 4;
    [SerializeField] private float wobbleElasticity = 1f;
    
    private Vector3 originalScale;
    private Tween currentWobbleTween;
    
    private void Awake()
    {
        originalScale = transform.localScale;
    }
    
    public void PlayWobbleAnimation()
    {
        // Kill any existing wobble animation
        if (currentWobbleTween != null && currentWobbleTween.IsPlaying())
        {
            currentWobbleTween.Kill();
            transform.localScale = originalScale;
        }
        
        // Create new punch scale animation
        currentWobbleTween = transform.DOPunchScale(originalScale * (wobbleStrength - 1f), wobbleDuration, wobbleVibrato, wobbleElasticity)
            .SetEase(Ease.OutElastic)
            .OnComplete(() => transform.localScale = originalScale);
    }
}
