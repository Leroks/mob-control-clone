using DG.Tweening;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private float moveSpeed;
    private float lifetime = 12f;
    private float currentLifetime;

    public Vector3 GetVelocity()
    {
        return transform.forward * moveSpeed;
    }
    
    public void Initialize(float speed)
    {
        moveSpeed = speed;
        currentLifetime = lifetime;
    }

    private void OnEnable()
    {
        currentLifetime = lifetime;
    }

    private void Update()
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

    private void OnTriggerEnter(Collider other)
    {
        // Handle collision with enemies
        if (other.CompareTag("Enemy"))
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.TakeDamage();
                DestroyProjectile();
            }
        }
        
        // Handle collision with enemy castle
        if (other.CompareTag("EnemyCastle"))
        {
            other.GetComponent<EnemyCastle>().GetHit(1);
            ReturnToPool();
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
    }
    
    private void ReturnToPool()
    {
        if (ProjectilePool.Instance != null)
        {
            ProjectilePool.Instance.ReturnProjectile(this);
        }
    }
}
