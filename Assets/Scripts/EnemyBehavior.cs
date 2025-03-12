using UnityEngine;
using DG.Tweening;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private int health = 1;
    [SerializeField] private int damageToPlayer = 1;
    [SerializeField] private GameObject deathEffectPrefab;
    
    private float moveSpeed;
    private Vector3 targetPosition;
    private bool isActive;
    private Vector3 originalScale;
    
    public void Initialize(float speed, Vector3 target)
    {
        moveSpeed = speed;
        targetPosition = target;
        isActive = true;
        health = (transform.localScale.x > 1.5f) ? 3 : 1; // Big enemies have more health
        originalScale = transform.localScale;
        
        // Play spawn animation
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack);
    }
    
    void Update()
    {
        if (!isActive) return;
        
        // Move forward (in the direction the enemy is facing)
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        
        // Check if enemy has gone too far
        if (transform.position.z < -10f)
        {
            //ReturnToPool();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // if (!isActive) return;
        //
        // if (other.CompareTag("Player"))
        // {
        //     // Handle collision with player (projectile)
        //     isActive = false;
        //     
        //     // Get reference to the projectile
        //     ProjectileBehavior projectile = other.GetComponent<ProjectileBehavior>();
        //     if (projectile != null)
        //     {
        //         // Tell the projectile to destroy itself
        //         projectile.DestroyProjectile();
        //     }
        //     
        //     // Play death animation and return to pool
        //     transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => {
        //         ReturnToPool();
        //     });
        // }
    }
    
    // Called by projectiles when hit
    public void TakeDamage()
    {
        if (!isActive) return;
        
        health--;
        
        // Play hit feedback
        transform.DOShakeScale(0.2f, 0.3f, 5);
        
        if (health <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            // Enemy is destroyed
            isActive = false;
            
            // Spawn death effect if available
            if (deathEffectPrefab != null)
            {
                Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            }
            
            // Play death animation and return to pool
            transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => {
                ReturnToPool();
            });
        }
    }
    
    private void ReturnToPool()
    {
        isActive = false;
        if (EnemyPool.Instance != null)
        {
            EnemyPool.Instance.ReturnEnemy(this);
        }
    }
}
