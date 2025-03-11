using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int amount;
    [SerializeField] GameObject player;
    [SerializeField] float time;
    [SerializeField] Vector2 xMove;
    [SerializeField] bool isMoving;

    void Start()
    {
        if (isMoving) 
        {
            DoMoveRight();
        }
            
    }
    private void DoMoveLeft()
    {
        transform.DOMoveX(xMove.x, time).OnComplete((DoMoveRight));
    }
    private void DoMoveRight()
    {
        transform.DOMoveX(xMove.y, time).OnComplete((DoMoveLeft));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the door");
            GameObject player = other.gameObject;
            
                for(int i = 0; i < amount-1; i++)
                {
                    Vector3 newSpawnPoint = player.transform.position;
                    float spawnX = Random.Range(-4, 4);
                    float spawnZ = Random.Range(-2, 2);
                    newSpawnPoint.x += spawnX;
                    newSpawnPoint.z += spawnZ;

                    ProjectileBehavior projectile = ProjectilePool.Instance.GetProjectile();
                    projectile.transform.position = newSpawnPoint;
                    projectile.Initialize(1, 2f, null, 5f);
                }
            
        }
    }
}
