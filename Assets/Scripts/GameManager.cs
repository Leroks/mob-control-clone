using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    public PlayerController playerController;
    public float levelLength = 100f;
    public int targetCrowdSize = 50;
    
    [Header("UI References")]
    public TMPro.TextMeshProUGUI crowdSizeText;
    public TMPro.TextMeshProUGUI levelProgressText;
    
    private bool isGameOver = false;
    private float startZ;
    
    void Awake()
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
    
    void Start()
    {
        startZ = playerController.transform.position.z;
        UpdateUI();
    }
    
    void Update()
    {
        if (!isGameOver)
        {
            UpdateUI();
            CheckLevelProgress();
        }
    }
    
    void UpdateUI()
    {
        if (crowdSizeText != null)
        {
            crowdSizeText.text = $"Crowd: {playerController.GetCrowdSize()}";
        }
        
        if (levelProgressText != null)
        {
            float progress = Mathf.Clamp01((playerController.transform.position.z - startZ) / levelLength);
            levelProgressText.text = $"Progress: {(progress * 100):F0}%";
        }
    }
    
    void CheckLevelProgress()
    {
        float progress = (playerController.transform.position.z - startZ) / levelLength;
        
        if (progress >= 1f)
        {
            WinLevel();
        }
    }
    
    public void WinLevel()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("Level Complete!");
            // Implement level completion logic
        }
    }
    
    public void LoseLevel()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("Level Failed!");
            // Implement level failure logic
        }
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
