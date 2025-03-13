using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;

public class Barricade : MonoBehaviour
{
    [Header("Barricade Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private TextMeshProUGUI healthText;
    
    [Header("Visual Feedback")]
    [SerializeField] private float hitShakeDuration = 0.2f;
    [SerializeField] private float hitShakeStrength = 0.3f;
    [SerializeField] private int hitShakeVibrato = 5;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.white;
    
    private Material material;
    private Color originalColor;
    
    private int currentHealth;
    private Vector3 originalScale;
    private Tweener currentShakeTween;

    private void Start()
    {
        currentHealth = maxHealth;
        originalScale = transform.localScale;
        UpdateHealthDisplay();
        
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            originalColor = material.color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the projectile and destroy it
            ProjectileBehavior projectile = other.GetComponent<ProjectileBehavior>();
            if (projectile != null)
            {
                projectile.DestroyProjectile();
                TakeDamage();
            }
        }
    }
    
    public void TakeDamage()
    {
        currentHealth--;
        UpdateHealthDisplay();
        PlayHitEffect();
        
        if (currentHealth <= 0)
        {
            DestroyBarricade();
        }
    }
    
    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }
    
    private void PlayHitEffect()
    {
        // Shake
        if (currentShakeTween != null && currentShakeTween.IsPlaying())
        {
            currentShakeTween.Kill();
        }
        
        transform.localScale = originalScale;
        currentShakeTween = transform.DOShakeScale(hitShakeDuration, hitShakeStrength, hitShakeVibrato)
            .OnComplete(() => transform.localScale = originalScale);
            
        // Flash effect
        if (material != null)
        {
            DOTween.Kill(material);
            
            material.DOColor(flashColor, flashDuration)
                .SetEase(Ease.OutFlash)
                .OnComplete(() => {
                    material.DOColor(originalColor, flashDuration)
                        .SetEase(Ease.InOutQuad);
                });
        }
    }
    
    private void DestroyBarricade()
    {
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() => { Destroy(gameObject);});
    }
}
