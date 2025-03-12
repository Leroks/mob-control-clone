using DG.Tweening;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private int damageAmount;
    private float explosionRadius;
    private GameObject explosionEffect;
    private float moveSpeed;
    private float lifetime = 12f;
    private float currentLifetime;
    
    public void Initialize(int damage, float radius, GameObject effectPrefab, float speed)
    {
        damageAmount = damage;
        explosionRadius = radius;
        explosionEffect = effectPrefab;
        moveSpeed = speed;
        currentLifetime = lifetime;
    }
    
    void OnEnable()
    {
        currentLifetime = lifetime;
    }
    
    void Update()
    {
        // Continuous forward movement
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        
        // Handle lifetime
        currentLifetime -= Time.deltaTime;
        if (currentLifetime <= 0)
        {
            ReturnToPool();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        bool shouldExplode = false;
        
        // Handle collision with enemies
        if (other.CompareTag("Enemy"))
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.TakeDamage();
                DestroyProjectile();
            }
            shouldExplode = true;
        }
        
        // Handle collision with enemy castle
        if (other.CompareTag("EnemyCastle"))
        {
            other.GetComponent<EnemyCastle>().GetHit(1);
            shouldExplode = true;
        }
        
        if (shouldExplode)
        {
            // Spawn explosion effect
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
        }
    }
    
    // Called by enemies to destroy the projectile when they collide
    public void DestroyProjectile()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        // Play destruction animation
        transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => {
            ReturnToPool();
        });
        
        // Spawn explosion effect immediately
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
    }
    
    private void ReturnToPool()
    {
        if (ProjectilePool.Instance != null)
        {
            ProjectilePool.Instance.ReturnProjectile(this);
        }
    }
}
