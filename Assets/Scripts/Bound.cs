using UnityEngine;

public class Bound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Handle projectiles
        if (other.CompareTag("Player"))
        {
            ProjectileBehavior projectile = other.GetComponent<ProjectileBehavior>();
            if (projectile != null)
            {
                ProjectilePool.Instance.ReturnProjectile(projectile);
            }
        }
        
        // Handle enemies
        if (other.CompareTag("Enemy"))
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                EnemyPool.Instance.ReturnEnemy(enemy);
            }
        }
    }
}
