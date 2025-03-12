using UnityEngine;

public class TubeEntrance : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private TubeExit targetExit;
    
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem entranceEffect;
    private WobbleEffect wobbleEffect;

    private void Start()
    {
        wobbleEffect = GetComponent<WobbleEffect>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && targetExit != null)
        {
            ProjectileBehavior projectile = other.GetComponent<ProjectileBehavior>();
            if (projectile != null)
            {
                // Play wobble effect on entrance tube
                if (wobbleEffect != null)
                {
                    wobbleEffect.PlayWobbleAnimation();
                }

                // Store projectile info before returning to pool
                float speed = projectile.GetVelocity().magnitude;
                
                // Play entrance effect
                if (entranceEffect != null)
                {
                    Instantiate(entranceEffect, other.transform.position, Quaternion.identity);
                }

                // Return to pool
                ProjectilePool.Instance.ReturnProjectile(projectile);
                
                // Spawn at exit
                ProjectileBehavior newProjectile = ProjectilePool.Instance.GetProjectile();
                if (newProjectile != null)
                {
                    // Keep the same Y position, only change X and Z
                    Vector3 exitPos = targetExit.transform.position;
                    exitPos.y = other.transform.position.y;
                    newProjectile.transform.position = exitPos;
                    newProjectile.transform.forward = -targetExit.transform.up;
                    newProjectile.Initialize(projectile.CrowdReductionAmount, 
                                          projectile.ExplosionRadius, 
                                          projectile.ExplosionEffectPrefab, 
                                          speed);
                    
                    // Tell exit to play effects
                    targetExit.PlayExitEffects();
                }
            }
        }
}
}
