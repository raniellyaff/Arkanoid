using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    
    void Start()
    {
        UpdateUI();
    }
    
    void Update()
    {
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (ScoreManager.Instance == null) return;
        
        if (scoreText != null)
        {
            int score = ScoreManager.Instance.GetScore();
            scoreText.text = $"Score: {score}";
        }
        
        if (livesText != null)
        {
            int lives = ScoreManager.Instance.GetLives();
            livesText.text = $"Vidas: {lives}";
        }
    }
}