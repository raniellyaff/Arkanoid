using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI victoryMessageText;
    
    void Start()
    {
        // Mostra a pontuação final
        if (finalScoreText != null && ScoreManager.Instance != null)
        {
            finalScoreText.text = $"🏆 Pontuação Final: {ScoreManager.Instance.GetScore()}";
        }
        
        if (victoryMessageText != null)
        {
            victoryMessageText.text = "🎉 PARABÉNS!\nVocê venceu o jogo!";
        }
        
        // Pausa o jogo
        Time.timeScale = 0f;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        
        SceneManager.LoadScene("Inicial");
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}