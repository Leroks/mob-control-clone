using UnityEngine;

public class Bound : MonoBehaviour
{
    [SerializeField] private bool isEndGameBound = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isEndGameBound && other.CompareTag("Enemy"))
        {
            GameManager.Instance.LoseGame();
            return;
        }
        if (!isEndGameBound && other.CompareTag("Player"))
        {
            ProjectileBehavior projectile = other.GetComponent<ProjectileBehavior>();
            if (projectile != null)
            {
                ProjectilePool.Instance.ReturnProjectile(projectile);
            }
        }
        
        if (!isEndGameBound && other.CompareTag("Enemy"))
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                EnemyPool.Instance.ReturnEnemy(enemy);
            }
        }
    }
}
