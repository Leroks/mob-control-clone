using System;
using UnityEngine;
using DG.Tweening;

public class TubeEntrance : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private TubeExit targetExit;
    [SerializeField] private float teleportDuration = 0.3f;
    
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem entranceEffect;
    [SerializeField] private Color teleportColor = Color.cyan;
    private WobbleEffect wobbleEffect;

    private void Start()
    {
        wobbleEffect = GetComponent<WobbleEffect>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && targetExit != null)
        {
            // Play wobble effect on entrance tube
            if (wobbleEffect != null)
            {
                wobbleEffect.PlayWobbleAnimation();
            }
            
            TeleportObject(other.gameObject);
        }
    }
    
    private void TeleportObject(GameObject obj)
    {
        // Store original scale
        Vector3 originalScale = obj.transform.localScale;
        
        // Use default movement speed
        float defaultSpeed = 5f;
        
        // Play entrance effect
        if (entranceEffect != null)
        {
            Instantiate(entranceEffect, obj.transform.position, Quaternion.identity);
        }
        
        // Sequence for smooth teleport animation
        Sequence teleportSequence = DOTween.Sequence();
        
        // Shrink and spin at entrance
        teleportSequence.Append(obj.transform.DOScale(Vector3.zero, teleportDuration / 2)
            .SetEase(Ease.InBack));
        teleportSequence.Join(obj.transform.DORotate(new Vector3(0, 360, 0), teleportDuration / 2, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InCubic));
        
        // Move to exit point (instantly, while invisible)
        teleportSequence.AppendCallback(() => {
            // Position at exit
            obj.transform.position = targetExit.transform.position;
            
            // Align with exit direction
            obj.transform.forward = -targetExit.transform.up;
            
            // Tell exit to play effects and set movement
            targetExit.PlayExitEffects(obj, originalScale, defaultSpeed);
        });
    }
}
