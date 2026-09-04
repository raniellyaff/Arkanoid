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
            scoreText.text = $"Score: {ScoreManager.Instance.GetScore()}";
        }
        
        if (livesText != null)
        {
            livesText.text = $"Vidas: {ScoreManager.Instance.GetLives()}";
        }
    }
}