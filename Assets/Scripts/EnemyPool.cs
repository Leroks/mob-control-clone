using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance { get; private set; }
    
    [Header("Pool Settings")]
    public GameObject enemyPrefab;
    public int poolSize = 20;
    
    private Queue<EnemyBehavior> pool;
    private List<EnemyBehavior> activeEnemies;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Initialize()
    {
        pool = new Queue<EnemyBehavior>();
        activeEnemies = new List<EnemyBehavior>();
        
        // Create initial pool
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewEnemy();
        }
    }
    
    void CreateNewEnemy()
    {
        GameObject obj = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, transform);
        EnemyBehavior enemy = obj.GetComponent<EnemyBehavior>();
        if (enemy == null)
        {
            enemy = obj.AddComponent<EnemyBehavior>();
        }
        
        obj.SetActive(false);
        pool.Enqueue(enemy);
    }
    
    public EnemyBehavior GetEnemy()
    {
        if (pool.Count == 0)
        {
            CreateNewEnemy();
        }
        
        EnemyBehavior enemy = pool.Dequeue();
        enemy.GetComponent<CapsuleCollider>().enabled = true;
        activeEnemies.Add(enemy);
        enemy.gameObject.SetActive(true);
        return enemy;
    }
    
    public void ReturnEnemy(EnemyBehavior enemy)
    {
        enemy.gameObject.SetActive(false);
        activeEnemies.Remove(enemy);
        pool.Enqueue(enemy);
    }
    
    public void ReturnAllEnemies()
    {
        while (activeEnemies.Count > 0)
        {
            ReturnEnemy(activeEnemies[0]);
        }
    }
}
