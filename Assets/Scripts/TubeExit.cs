using UnityEngine;

public class TubeExit : MonoBehaviour
{
    [Header("Visual Effects")]
    private WobbleEffect wobbleEffect;

    private void Start()
    {
       wobbleEffect = GetComponent<WobbleEffect>();
    }

    public void PlayExitEffects()
    {
        if (wobbleEffect != null)
        {
            wobbleEffect.PlayWobbleAnimation();
        }
    }
}
