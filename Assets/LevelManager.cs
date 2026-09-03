using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [Header("UI - Level Complete")]
    public GameObject levelCompletePanel;
    public TMPro.TextMeshProUGUI levelCompleteText;
    
    [Header("Configurações")]
    public float delayBeforeNextLevel = 2f; // Tempo antes de ir para a próxima fase
    
    private bool isLevelComplete = false;
    private int currentLevel = 1;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        
        // Verifica se é uma fase do jogo
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Scene1" || sceneName == "Scene2")
        {
            currentLevel = sceneName == "Scene1" ? 1 : 2;
            Debug.Log($"🎮 Nível {currentLevel} iniciado!");
        }
    }
    
    void Update()
    {
        // Verifica se completou a fase (apenas nas fases do jogo)
        string sceneName = SceneManager.GetActiveScene().name;
        if ((sceneName == "Scene1" || sceneName == "Scene2") && !isLevelComplete)
        {
            CheckLevelComplete();
        }
    }
    
    void CheckLevelComplete()
    {
        // Conta quantos blocos ainda existem
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        
        // Se não tiver mais blocos, completou a fase
        if (bricks.Length == 0)
        {
            CompleteLevel();
        }
    }
    
    void CompleteLevel()
    {
        isLevelComplete = true;
        
        // Pausa o jogo
        Time.timeScale = 0f;
        
        // Mostra o painel de conclusão
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }
        
        // Atualiza o texto
        if (levelCompleteText != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            int levelNum = sceneName == "Scene1" ? 1 : 2;
            levelCompleteText.text = $"🎉 Fase {levelNum} Concluída!";
        }
        
        // Para a bola (opcional)
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        
        Debug.Log($"🎉 Fase {SceneManager.GetActiveScene().name} concluída!");
        
        // Carrega a próxima fase automaticamente após um tempo
        // OU espera o jogador clicar no botão
        // Invoke("NextLevel", delayBeforeNextLevel);
    }
    
    public void NextLevel()
    {
        // Volta o tempo normal
        Time.timeScale = 1f;
        
        // Reseta a flag
        isLevelComplete = false;
        
        // Esconde o painel
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
        
        // Pega o nome da cena atual
        string currentScene = SceneManager.GetActiveScene().name;
        
        // Define a próxima cena
        string nextScene = "";
        
        if (currentScene == "Scene1")
            nextScene = "Scene2";
        else if (currentScene == "Scene2")
            nextScene = "Vitoria";
        
        // Carrega a próxima cena
        if (!string.IsNullOrEmpty(nextScene))
        {
            // Reseta as vidas para a próxima fase (mantém o score)
            if (ScoreManager.Instance != null)
            {
                // Mantém o score, reseta as vidas
                ScoreManager.Instance.ResetLivesForNewLevel();
            }
            
            Debug.Log($"🔄 Carregando: {nextScene}");
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogError("Próxima cena não definida!");
        }
    }
    
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        isLevelComplete = false;
        
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
        
        // Reseta o score e vidas
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}