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
            ProjectileBehavior originalProjectile = other.GetComponent<ProjectileBehavior>();
            if (originalProjectile != null)
            {
                float speed = originalProjectile.GetVelocity().magnitude;
                
                for(int i = 0; i < multiplierAmount-1; i++)
                {
                    Vector3 newSpawnPoint = other.transform.position;
                    float spawnX = Random.Range(-0.9f, 0.9f);
                    float spawnZ = Random.Range(1, 2);
                    newSpawnPoint.x += spawnX;
                    newSpawnPoint.z += spawnZ;

                    ProjectileBehavior projectile = ProjectilePool.Instance.GetProjectile();
                    projectile.transform.position = newSpawnPoint;
                    projectile.transform.forward = Vector3.forward;
                    projectile.Initialize(speed);
                }
            }
        }
    }
}
