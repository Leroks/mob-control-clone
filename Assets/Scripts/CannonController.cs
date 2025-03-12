using UnityEngine;
using DG.Tweening;

public class CannonController : MonoBehaviour
{
    [Header("Cannon Settings")]
    public float moveSpeed = 5f;
    public float projectileSpeed = 15f;
    public float fireRate = 0.1f;
    public Transform firePoint;
    
    [Header("Impact Settings")]
    public int crowdReductionAmount = 5;
    public float explosionRadius = 2f;
    public GameObject explosionEffectPrefab;
    
    private float nextFireTime;
    private bool isMouseHeld;
    private Camera mainCamera;
    private Tweener currentWobbleTween;
    private WobbleEffect wobbleEffect;
    
    private void Start()
    {
        mainCamera = Camera.main;
        DOTween.SetTweensCapacity(500, 50);
        wobbleEffect = GetComponent<WobbleEffect>();
    }
    
    private void Update()
    {
        HandleMouseInput();
        FollowMouseX();
    }
    
    private void HandleMouseInput()
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

    private void FollowMouseX()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Lerp(newPosition.x, worldPos.x, moveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    private void Fire()
    {
        ProjectileBehavior projectile = ProjectilePool.Instance.GetProjectile();
        if (projectile != null)
        {
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = Quaternion.identity;
            projectile.Initialize(crowdReductionAmount, explosionRadius, explosionEffectPrefab, projectileSpeed);
            
            wobbleEffect.PlayWobbleAnimation();
        }
    }
}
