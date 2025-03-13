using DG.Tweening;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private float moveSpeed;

    public Vector3 GetVelocity()
    {
        return transform.forward * moveSpeed;
    }
    
    public void Initialize(float speed)
    {
        moveSpeed = speed;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.TakeDamage();
                DestroyProjectile();
            }
        }
        
        if (other.CompareTag("EnemyCastle"))
        {
            other.GetComponent<EnemyCastle>().GetHit(1);
            ReturnToPool();
        }
    }
    
    public void DestroyProjectile()
    {
        GetComponent<CapsuleCollider>().enabled = false;
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
