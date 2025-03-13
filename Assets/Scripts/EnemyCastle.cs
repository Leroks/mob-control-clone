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
    [SerializeField] private float spawnSpreadX = 2f;
    [SerializeField] private float spawnSpreadZ = 3f;
    
    [Header("Spawn Events")]
    [SerializeField] private Vector2[] spawnEvents; // x: time, y: normalEnemyCount
    
    [Header("Visual Feedback")]
    [SerializeField] private float spawnEffectDuration = 0.3f;
    [SerializeField] private float spawnEffectStrength = 1.2f;
    [SerializeField] private int spawnEffectVibrato = 4;
    [SerializeField] private float spawnEffectElasticity = 1f;
    
    [Header("Hit Effect")]
    [SerializeField] private float hitShakeDuration = 0.15f;
    [SerializeField] private float hitShakeStrength = 0.2f;
    [SerializeField] private int hitShakeVibrato = 8;
    [SerializeField] private float hitShakeRandomness = 45f;
    
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
                SpawnWave((int)spawnEvents[i].y);
                spawnEvents[i].x = 0;
            }
        }
        
        healthText.text = health.ToString();
    }
    
    private void SpawnWave(int normalEnemyCount)
    {
        for (int i = 0; i < normalEnemyCount; i++)
        {
            SpawnEnemy();
        }
        
        PlaySpawnEffect();
    }
    
    private void SpawnEnemy()
    {
        EnemyBehavior enemy = EnemyPool.Instance.GetEnemy();
        if (enemy != null)
        {
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.x += Random.Range(-spawnSpreadX, spawnSpreadX);
            spawnPosition.z += Random.Range(-spawnSpreadZ, spawnSpreadZ);
            
            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
            
            float speed = normalEnemySpeed;
            enemy.Initialize(speed, transform.position + Vector3.back * 10f);
        }
    }
    
    private void PlaySpawnEffect()
    {
        if (currentSpawnTween != null && currentSpawnTween.IsPlaying())
        {
            currentSpawnTween.Kill();
        }
        
        transform.localScale = originalScale;
        
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
        
        Quaternion originalRotation = transform.localRotation;
        
        transform.DOShakeRotation(hitShakeDuration, hitShakeStrength * 15f, hitShakeVibrato, hitShakeRandomness)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => transform.localRotation = originalRotation);
    }
}
