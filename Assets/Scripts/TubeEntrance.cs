using UnityEngine;
using System.Collections;

public class TubeEntrance : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private TubeExit targetExit;
    [SerializeField] private float teleportDelay = 1.0f;
    
    [Header("Visual Effects")]
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
                if (wobbleEffect != null)
                {
                    wobbleEffect.PlayWobbleAnimation();
                }
                
                float speed = projectile.GetVelocity().magnitude;
                
                
                float yPosition = other.transform.position.y;
                
                ProjectilePool.Instance.ReturnProjectile(projectile);
                
                StartCoroutine(DelayedSpawn(speed, yPosition));
            }
        }
    }
    
    private IEnumerator DelayedSpawn(float speed, float yPosition)
    {
        yield return new WaitForSeconds(teleportDelay);
        
        // Spawn at exit
        ProjectileBehavior newProjectile = ProjectilePool.Instance.GetProjectile();
        if (newProjectile != null)
        {
            // Keep the same Y
            Vector3 exitPos = targetExit.transform.position;
            exitPos.y = yPosition;
            newProjectile.transform.position = exitPos;
            newProjectile.transform.forward = -targetExit.transform.up;
            newProjectile.Initialize(speed);
            
            targetExit.PlayExitEffects();
        }
    }
}
