using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private int damageAmount;
    private float explosionRadius;
    private GameObject explosionEffect;
    private float moveSpeed;
    private float lifetime = 5f;
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
        // Check for crowd impact
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                
            }
        }
        
        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        ReturnToPool();
    }
    
    private void ReturnToPool()
    {
        if (ProjectilePool.Instance != null)
        {
            ProjectilePool.Instance.ReturnProjectile(this);
        }
    }
}
