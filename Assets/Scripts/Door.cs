using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int amount;
    [SerializeField] GameObject player;
    [SerializeField] float time;
    [SerializeField] float moveAmountX;
    [SerializeField] bool isMoving;
    [SerializeField] TextMeshProUGUI multiplierText;

    void Start()
    {
        if (isMoving) 
        {
            DoMoveHorizontalLoop();
        }
        
        multiplierText.text = "X"+ amount.ToString();
    }

    private void DoMoveHorizontalLoop()
    {
        transform.DOMoveX(transform.position.x + moveAmountX, time).SetLoops(-1, LoopType.Yoyo);
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
                    float spawnX = Random.Range(-1, 1);
                    float spawnZ = Random.Range(1, 2);
                    newSpawnPoint.x += spawnX;
                    newSpawnPoint.z += spawnZ;

                    ProjectileBehavior projectile = ProjectilePool.Instance.GetProjectile();
                    projectile.transform.position = newSpawnPoint;
                    projectile.Initialize(1, 2f, null, 5f);
                }
            
        }
    }
}
