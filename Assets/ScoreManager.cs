using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<ScoreManager>();
                
                if (_instance == null)
                {
                    GameObject go = new GameObject("ScoreManager");
                    _instance = go.AddComponent<ScoreManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    
    private int score = 0;
    private int lives = 3;
    private int maxLives = 5;
    
    [Header("Configurações")]
    public int initialLives = 3;
    
    [Header("Game Over - UI")]
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI gameOverScoreText;
    
    private bool isGameOver = false;
    private bool isInitialized = false;
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            ResetScore();
        }
        
        FindAndConnectUI();
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAndConnectUI();
        
        if (scene.name == "Scene1" || scene.name == "Scene2")
        {
            lives = initialLives;
            isGameOver = false;
            Time.timeScale = 1f;
            
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);
        }
    }
    
    void FindAndConnectUI()
    {
        if (gameOverPanel == null)
        {
            GameObject panel = GameObject.Find("GameOverPanel");
            if (panel != null)
            {
                gameOverPanel = panel;
            }
        }
        
        if (gameOverScoreText == null && gameOverPanel != null)
        {
            foreach (var t in gameOverPanel.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            {
                if (t.name == "GameOverScoreText" || t.name == "FinalScoreText" || t.name.Contains("Score"))
                {
                    gameOverScoreText = t;
                    break;
                }
            }
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
        
        if (lives <= 0)
        {
            lives = 0;
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
        Time.timeScale = 1f;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
    
    public void ResetLivesForNewLevel()
    {
        lives = initialLives;
        isGameOver = false;
        Time.timeScale = 1f;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
    
    void GameOver()
    {
        isGameOver = true;
        
        FindAndConnectUI();
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            CreateGameOverPanel();
        }
        
        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = $"Score Final: {score}";
        }
        
        Time.timeScale = 0f;
    }
    
    void CreateGameOverPanel()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null) return;
        
        GameObject panel = new GameObject("GameOverPanel");
        panel.transform.SetParent(canvas.transform);
        panel.transform.SetSiblingIndex(canvas.transform.childCount - 1);
        
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(450, 350);
        rect.anchoredPosition = Vector2.zero;
        
        UnityEngine.UI.Image image = panel.AddComponent<UnityEngine.UI.Image>();
        image.color = new Color(0, 0, 0, 0.9f);
        
        // Texto "GAME OVER"
        GameObject titleObj = new GameObject("GameOverText");
        titleObj.transform.SetParent(panel.transform);
        TMPro.TextMeshProUGUI titleTmp = titleObj.AddComponent<TMPro.TextMeshProUGUI>();
        titleTmp.text = "GAME OVER";
        titleTmp.fontSize = 56;
        titleTmp.color = Color.red;
        titleTmp.alignment = TMPro.TextAlignmentOptions.Center;
        titleTmp.fontStyle = TMPro.FontStyles.Bold;
        
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.5f);
        titleRect.anchorMax = new Vector2(0.5f, 0.5f);
        titleRect.sizeDelta = new Vector2(350, 80);
        titleRect.anchoredPosition = new Vector2(0, 100);
        
        // Texto do score
        GameObject textObj = new GameObject("GameOverScoreText");
        textObj.transform.SetParent(panel.transform);
        TMPro.TextMeshProUGUI tmp = textObj.AddComponent<TMPro.TextMeshProUGUI>();
        tmp.text = $"Score Final: {score}";
        tmp.fontSize = 36;
        tmp.color = Color.white;
        tmp.alignment = TMPro.TextAlignmentOptions.Center;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.sizeDelta = new Vector2(350, 60);
        textRect.anchoredPosition = new Vector2(0, 30);
        
        // Botão Reiniciar
        GameObject btnObj = new GameObject("RestartButton");
        btnObj.transform.SetParent(panel.transform);
        
        UnityEngine.UI.Image btnImage = btnObj.AddComponent<UnityEngine.UI.Image>();
        btnImage.color = new Color(0.2f, 0.6f, 0.2f);
        
        UnityEngine.UI.Button btn = btnObj.AddComponent<UnityEngine.UI.Button>();
        
        GameObject btnTextObj = new GameObject("Text (TMP)");
        btnTextObj.transform.SetParent(btnObj.transform);
        TMPro.TextMeshProUGUI btnTmp = btnTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        btnTmp.text = "Reiniciar";
        btnTmp.fontSize = 32;
        btnTmp.color = Color.white;
        btnTmp.alignment = TMPro.TextAlignmentOptions.Center;
        btnTmp.fontStyle = TMPro.FontStyles.Bold;
        
        RectTransform btnRect = btnObj.GetComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.sizeDelta = new Vector2(220, 60);
        btnRect.anchoredPosition = new Vector2(0, -60);
        
        RectTransform btnTextRect = btnTextObj.GetComponent<RectTransform>();
        btnTextRect.anchorMin = new Vector2(0, 0);
        btnTextRect.anchorMax = new Vector2(1, 1);
        btnTextRect.offsetMin = Vector2.zero;
        btnTextRect.offsetMax = Vector2.zero;
        
        btn.onClick.AddListener(() => { RestartGame(); });
        
        gameOverPanel = panel;
        gameOverScoreText = tmp;
        
        panel.SetActive(true);
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        ResetScore();
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        SceneManager.LoadScene("Scene1");
    }
}