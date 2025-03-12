using UnityEngine;

public class TubeExit : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem exitEffect;
    private WobbleEffect wobbleEffect;

    private void Start()
    {
       wobbleEffect = GetComponent<WobbleEffect>();
    }

    public void PlayExitEffects()
    {
        // Play exit particle effect
        if (exitEffect != null)
        {
            Instantiate(exitEffect, transform.position, Quaternion.identity);
        }
        
        // Play wobble effect on exit tube
        if (wobbleEffect != null)
        {
            wobbleEffect.PlayWobbleAnimation();
        }
    }
}
