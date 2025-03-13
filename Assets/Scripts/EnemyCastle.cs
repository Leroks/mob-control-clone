using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemyCastle : MonoBehaviour
{
    [Header("Castle Settings")]
    [SerializeField] private ParticleSystem castleParticular;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int health = 100;
    
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float normalEnemySpeed = 5f;
    [SerializeField] private float bigEnemySpeed = 3f;
    [SerializeField] private float spawnSpreadX = 2f;
    [SerializeField] private float spawnSpreadZ = 3f;
    
    [Header("Spawn Events")]
    [SerializeField] private Vector3[] spawnEvents; // x: time, y: normalEnemyCount, z: bigEnemyCount
    
    [Header("Visual Feedback")]
    [SerializeField] private float spawnEffectDuration = 0.3f;
    [SerializeField] private float spawnEffectStrength = 1.2f;
    [SerializeField] private int spawnEffectVibrato = 4;
    [SerializeField] private float spawnEffectElasticity = 1f;
    
    private float spawnTimer;
    private Vector3 originalScale;
    private Tweener currentSpawnTween;
    
    private void Start()
    {
        spawnTimer = 0;
        originalScale = transform.localScale;
    }
    
    private void Update()
    {
        if (health <= 0)
        { 
            Destroy(gameObject);
            GameManager.Instance.WinGame();
            return;
        }
        
        spawnTimer += Time.deltaTime;
        
        for (int i = 0; i < spawnEvents.Length; i++)
        {
            if (spawnEvents[i].x == (int)spawnTimer)
            {
                SpawnWave((int)spawnEvents[i].y, (int)spawnEvents[i].z);
                spawnEvents[i].x = 0;
            }
        }
        
        healthText.text = health.ToString();
    }
    
    private void SpawnWave(int normalEnemyCount, int bigEnemyCount)
    {
        // Spawn normal enemies
        for (int i = 0; i < normalEnemyCount; i++)
        {
            SpawnEnemy(false);
        }
        
        // Spawn big enemies
        for (int i = 0; i < bigEnemyCount; i++)
        {
            SpawnEnemy(true);
        }
        
        // Play spawn effect
        PlaySpawnEffect();
    }
    
    private void SpawnEnemy(bool isBig)
    {
        EnemyBehavior enemy = EnemyPool.Instance.GetEnemy();
        if (enemy != null)
        {
            // Random position within spread range
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.x += Random.Range(-spawnSpreadX, spawnSpreadX);
            spawnPosition.z += Random.Range(-spawnSpreadZ, spawnSpreadZ);
            
            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
            
            float speed = isBig ? bigEnemySpeed : normalEnemySpeed;
            enemy.Initialize(speed, transform.position + Vector3.back * 10f);
            
            if (isBig)
            {
                enemy.transform.localScale = Vector3.one * 2f; // Make big enemies larger
            }
        }
    }
    
    private void PlaySpawnEffect()
    {
        // Kill any existing spawn animation
        if (currentSpawnTween != null && currentSpawnTween.IsPlaying())
        {
            currentSpawnTween.Kill();
        }
        
        // Reset scale before starting new animation
        transform.localScale = originalScale;
        
        // Create new punch scale animation
        currentSpawnTween = transform.DOPunchScale(originalScale * (spawnEffectStrength - 1f), spawnEffectDuration, spawnEffectVibrato, spawnEffectElasticity)
            .SetEase(Ease.OutElastic)
            .OnComplete(() => transform.localScale = originalScale);
    }
    
    public void GetHit(int damage)
    {
        if(GameManager.Instance.IsGameOver) return;
        health -= damage;
        CastleHitEffect();
    }
    
    private void CastleHitEffect()
    {
        castleParticular.Play();
    }
}
