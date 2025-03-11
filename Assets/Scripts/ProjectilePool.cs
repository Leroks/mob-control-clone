using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }
    
    [Header("Pool Settings")]
    public GameObject projectilePrefab;
    public int poolSize = 20;
    public bool expandable = false;
    
    private Queue<ProjectileBehavior> pool;
    private List<ProjectileBehavior> activeProjectiles;
    
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
        pool = new Queue<ProjectileBehavior>();
        activeProjectiles = new List<ProjectileBehavior>();
        
        // Create initial pool
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewProjectile();
        }
    }
    
    void CreateNewProjectile()
    {
        GameObject obj = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity, transform);
        ProjectileBehavior projectile = obj.GetComponent<ProjectileBehavior>();
        if (projectile == null)
        {
            projectile = obj.AddComponent<ProjectileBehavior>();
        }
        
        // Set up trigger collider
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        
        obj.SetActive(false);
        pool.Enqueue(projectile);
    }
    
    public ProjectileBehavior GetProjectile()
    {
        if (pool.Count == 0 && !expandable)
        {
            Debug.LogWarning("Projectile pool is empty and not expandable!");
            return null;
        }
        
        ProjectileBehavior projectile;
        if (pool.Count == 0)
        {
            CreateNewProjectile();
        }
        
        projectile = pool.Dequeue();
        activeProjectiles.Add(projectile);
        projectile.gameObject.SetActive(true);
        return projectile;
    }
    
    public void ReturnProjectile(ProjectileBehavior projectile)
    {
        projectile.gameObject.SetActive(false);
        activeProjectiles.Remove(projectile);
        pool.Enqueue(projectile);
    }
    
    public void ReturnAllProjectiles()
    {
        while (activeProjectiles.Count > 0)
        {
            ReturnProjectile(activeProjectiles[0]);
        }
    }
}
