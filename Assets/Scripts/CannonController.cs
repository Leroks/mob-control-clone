using UnityEngine;
using DG.Tweening;

public class CannonController : MonoBehaviour
{
    [Header("Cannon Settings")]
    public float moveSpeed = 10f;
    public float projectileSpeed = 15f;
    public float fireRate = 0.1f;
    public Transform firePoint;
    
    [Header("Impact Settings")]
    public int crowdReductionAmount = 5;
    public float explosionRadius = 2f;
    public GameObject explosionEffectPrefab;
    
    [Header("Wobble Settings")]
    public float wobbleDuration = 0.3f;
    public float wobbleStrength = 1.2f;
    public int wobbleVibrato = 4;
    public float wobbleElasticity = 1f;
    
    private float nextFireTime;
    private bool isMouseHeld;
    private Camera mainCamera;
    private Vector3 originalScale;
    private Tweener currentWobbleTween;
    
    void Start()
    {
        mainCamera = Camera.main;
        originalScale = transform.localScale;
        DOTween.SetTweensCapacity(500, 50);
    }
    
    void Update()
    {
        HandleMouseInput();
        FollowMouseX();
    }
    
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseHeld = false;
        }
        
        if (isMouseHeld && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    void FollowMouseX()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Lerp(newPosition.x, worldPos.x, moveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
    
    void Fire()
    {
        ProjectileBehavior projectile = ProjectilePool.Instance.GetProjectile();
        if (projectile != null)
        {
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = Quaternion.identity;
            projectile.Initialize(crowdReductionAmount, explosionRadius, explosionEffectPrefab, projectileSpeed);
            
            // Play wobble animation
            PlayWobbleAnimation();
        }
    }
    
    void PlayWobbleAnimation()
    {
        // Kill any existing wobble animation
        if (currentWobbleTween != null && currentWobbleTween.IsPlaying())
        {
            currentWobbleTween.Kill();
        }
        
        // Reset scale before starting new animation
        transform.localScale = originalScale;
        
        // Create new punch scale animation
        currentWobbleTween = transform.DOPunchScale(originalScale * (wobbleStrength - 1f), wobbleDuration, wobbleVibrato, wobbleElasticity)
            .SetEase(Ease.OutElastic)
            .OnComplete(() => transform.localScale = originalScale);
    }
}
