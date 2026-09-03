using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    private int score = 0;
    private int lives = 3;
    private int maxLives = 5;
    
    [Header("Configurações")]
    public int initialLives = 3;
    
    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI finalScoreText;
    
    private bool isGameOver = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém entre as cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        lives = initialLives;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            Debug.Log("GameOverPanel desativado no Start");
        }
    }
    
    public void AddScore(int points)
    {
        if (isGameOver) return;
        score += points;
    }
    
    public void LoseLife()
    {
        if (isGameOver) return;
        
        lives--;
        Debug.Log($"Vidas restantes: {lives}");
        
        if (lives <= 0)
        {
            Debug.Log("Game Over! Ativando painel...");
            GameOver();
        }
    }
    
    public void AddLife()
    {
        if (isGameOver) return;
        
        if (lives < maxLives)
        {
            lives++;
        }
    }
    
    public int GetLives() { return lives; }
    public int GetScore() { return score; }
    
    public void ResetScore()
    {
        score = 0;
        lives = initialLives;
        isGameOver = false;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        Time.timeScale = 1f;
    }
    
    public void ResetLivesForNewLevel()
    {
        // Mantém o score, mas reseta as vidas
        lives = initialLives;
        isGameOver = false;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        Time.timeScale = 1f;
    }
    
    void GameOver()
    {
        isGameOver = true;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("GameOverPanel ativado!");
        }
        else
        {
            Debug.LogError("GameOverPanel não está conectado no ScoreManager!");
        }
        
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Score Final: {score}";
            Debug.Log($"Score final: {score}");
        }
        else
        {
            Debug.LogError("FinalScoreText não está conectado no ScoreManager!");
        }
        
        Time.timeScale = 0f;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        ResetScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Inicial");
    }
}