using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private int health = 1;
    
    private float moveSpeed;
    private Vector3 targetPosition;
    private bool isActive;
    private Vector3 originalScale;
    private Shader originalShader;
    
    public void Initialize(float speed, Vector3 target)
    {
        moveSpeed = speed;
        targetPosition = target;
        isActive = true;
        health = 1;
        originalScale = transform.localScale;
        dissolveProgress = 0.3f;
        
        if (originalShader == null)
        {
            Renderer renderer = transform.GetChild(1).GetComponent<Renderer>();
            originalShader = renderer.sharedMaterial.shader;
        }
        
        ResetMaterial();
        
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack);
    }

    private void Update()
    {
        if (!isActive) return;
        
        // Move forward
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
    
    public void TakeDamage()
    {
        if (!isActive) return;
        
        health--;
        
        // Hit feedback
        transform.DOShakeScale(0.2f, 0.3f, 5);
        
        if (health <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            isActive = false;
            
            StartCoroutine(DissolveAfterDelay());
        }
    }
    
    private void ResetMaterial()
    {
        Renderer renderer = transform.GetChild(1).GetComponent<Renderer>();
        if (renderer != null && originalShader != null)
        {
            renderer.sharedMaterial.shader = originalShader;
        }
    }
    
    private void ReturnToPool()
    {
        isActive = false;
        ResetMaterial();
        if (EnemyPool.Instance != null)
        {
            EnemyPool.Instance.ReturnEnemy(this);
        }
    }
    
    public Texture2D dissolveTexture;
    public Color dissolveColor = Color.white;
    public float dissolveTime = 1f;

    private Material material;
    private float dissolveProgress = 0.3f;
    
    private IEnumerator DissolveAfterDelay()
    {
        material = transform.GetChild(1).GetComponent<Renderer>().material;
        material.shader = Shader.Find("Custom/Dissolve");
        material.SetTexture("_DissolveTex", dissolveTexture);
        material.SetColor("_DissolveColor", dissolveColor);

        while (dissolveProgress < 1f)
        {
            dissolveProgress += Time.deltaTime / dissolveTime;
            material.SetFloat("_DissolveThreshold", dissolveProgress);
            yield return null;
        }

        ReturnToPool();
    }
}
