using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI defeatedText;
    [SerializeField] private TextMeshProUGUI victoryText;

    [Header("Text Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float scaleMultiplier = 1.2f;
    public bool IsGameOver { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void WinGame()
    {
        gameOverPanel.SetActive(true);
        TextAnimation(victoryText);
        EnemyPool.Instance.ReturnAllEnemies();
        IsGameOver = true;
    }
    
    public void LoseGame()
    {
        gameOverPanel.SetActive(true);
        TextAnimation(defeatedText);
        IsGameOver = true;
    }

    private void TextAnimation(TextMeshProUGUI animatedText)
    {
        animatedText.transform.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        
        animatedText.transform.localScale = Vector3.zero;
        animatedText.alpha = 0;
        
        sequence.Append(animatedText.transform.DOScale(scaleMultiplier, animationDuration).SetEase(Ease.OutBack));
        sequence.Join(animatedText.DOFade(1, animationDuration * 0.7f).SetEase(Ease.OutQuad));
        
        sequence.Append(animatedText.transform.DOScale(1f, animationDuration * 0.3f).SetEase(Ease.OutQuad));
    }
}
