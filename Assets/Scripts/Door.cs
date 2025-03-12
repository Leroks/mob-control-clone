using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;


public class Door : MonoBehaviour
{
    [SerializeField] private int multiplierAmount = 3;
    [SerializeField] private GameObject player;
    [SerializeField] private float timeToMove = 3f;
    [SerializeField] private float moveAmountX;
    [SerializeField] private bool isMoving;
    [SerializeField] private TextMeshProUGUI multiplierText;

    private void Start()
    {
        if (isMoving) 
        {
            DoMoveHorizontalLoop();
        }
        
        multiplierText.text = "X"+ multiplierAmount.ToString();
    }

    private void DoMoveHorizontalLoop()
    {
        transform.DOMoveX(transform.position.x + moveAmountX, timeToMove).SetLoops(-1, LoopType.Yoyo);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the door");
            GameObject player = other.gameObject;
            
                for(int i = 0; i < multiplierAmount-1; i++)
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
