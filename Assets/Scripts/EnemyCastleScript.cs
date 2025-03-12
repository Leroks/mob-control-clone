using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class EnemyCastleScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem castleParticular;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int health = 100;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject bigEnemy;

    [Header("Spawn:Time-EnemyAmount-BigEnemyAmount")]
    [SerializeField]
    private Vector3[] spawnEvents;
    private float spawnTimer;
    
    private void Start()
    {
        spawnTimer = 0;

    }

    private void Update()
    {
        if (health <= 0) { Destroy(gameObject); }
        spawnTimer += Time.deltaTime;

        for (int i = 0; i < spawnEvents.Length; i++)
        {
            if (spawnEvents[i].x == (int)spawnTimer)
            {
                Spawn((int)spawnEvents[i].y, (int)spawnEvents[i].z);
                spawnEvents[i].x = 0;
            }
        }
        healthText.text = health.ToString();
    }
    private void Spawn(int EnemyAmount, int bigEnemyAmount)
    {
        for (int i = 0; i < EnemyAmount; i++)
        {
            Vector3 newSpawnPoint = spawnPoint.position;
            int spawnX = Random.Range(-2, 2);
            int spawnZ = Random.Range(-3, 3);

            newSpawnPoint.z += spawnZ;
            newSpawnPoint.x += spawnX;
            GameObject enemySpawned = Instantiate(enemy, newSpawnPoint, Quaternion.identity);
            enemySpawned.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        for (int y = 0; y < bigEnemyAmount; y++)
        {
            Vector3 newSpawnPoint = spawnPoint.position;
            int spawnX = Random.Range(-2, 2);
            int spawnZ = Random.Range(-3, 3);

            newSpawnPoint.z += spawnZ;
            newSpawnPoint.x += spawnX;
            GameObject enemySpawned = Instantiate(bigEnemy, newSpawnPoint, Quaternion.identity);
            enemySpawned.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    public void GetHit(int damage)
    {
        health -= damage;
        CastleHitEffect();
    }

    private void CastleHitEffect()
    {
        castleParticular.Play();
    }
}
