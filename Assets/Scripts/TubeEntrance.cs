using UnityEngine;
using System.Collections;

public class TubeEntrance : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private TubeExit targetExit;
    [SerializeField] private float teleportDelay = 1.0f;
    
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
                
                float yPosition = other.transform.position.y;
                
                // Return to pool
                ProjectilePool.Instance.ReturnProjectile(projectile);
                
                // Start the delayed spawn coroutine
                StartCoroutine(DelayedSpawn(speed, yPosition));
            }
        }
    }
    
    private IEnumerator DelayedSpawn(float speed, float yPosition)
    {
        // Wait for the delay
        yield return new WaitForSeconds(teleportDelay);
        
        // Spawn at exit
        ProjectileBehavior newProjectile = ProjectilePool.Instance.GetProjectile();
        if (newProjectile != null)
        {
            // Keep the same Y position, only change X and Z
            Vector3 exitPos = targetExit.transform.position;
            exitPos.y = yPosition;
            newProjectile.transform.position = exitPos;
            newProjectile.transform.forward = -targetExit.transform.up;
            newProjectile.Initialize(speed);
            
            // Tell exit to play effects
            targetExit.PlayExitEffects();
        }
    }
}
