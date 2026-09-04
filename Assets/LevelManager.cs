using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [Header("UI - Level Complete")]
    public GameObject levelCompletePanel;
    public TMPro.TextMeshProUGUI levelCompleteText;
    public TMPro.TextMeshProUGUI levelFinalScoreText;
    
    private bool isLevelComplete = false;
    
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
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }
    
    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if ((sceneName == "Scene1" || sceneName == "Scene2") && !isLevelComplete)
        {
            CheckLevelComplete();
        }
    }
    
    void CheckLevelComplete()
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        
        if (bricks.Length == 0)
        {
            CompleteLevel();
        }
    }
    
    void CompleteLevel()
    {
        isLevelComplete = true;
        Time.timeScale = 0f;
        
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }
        
        if (levelCompleteText != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            string levelName = sceneName == "Scene1" ? "Fase 1" : "Fase 2";
            levelCompleteText.text = $"PARABENS!\n{levelName} Concluida!";
        }
        
        if (levelFinalScoreText != null && ScoreManager.Instance != null)
        {
            levelFinalScoreText.text = $"Score: {ScoreManager.Instance.GetScore()}";
        }
        
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }
    }
    
    public void NextLevel()
    {
        Time.timeScale = 1f;
        isLevelComplete = false;
        
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
        
        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = "";
        
        if (currentScene == "Scene1")
            nextScene = "Scene2";
        else if (currentScene == "Scene2")
            nextScene = "Vitoria";
        
        if (!string.IsNullOrEmpty(nextScene))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ResetLivesForNewLevel();
            }
            
            SceneManager.LoadScene(nextScene);
        }
    }
}