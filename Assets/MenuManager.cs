using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // Carrega a cena de Regras
        SceneManager.LoadScene("Regras");
    }
    
    public void GoToRules()
    {
        SceneManager.LoadScene("Regras");
    }
    
    public void GoToMenu()
    {
        // Reseta o score ao voltar para o menu
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("Inicial");
    }
    
    public void GoToPresentation()
    {
        SceneManager.LoadScene("Apresentacao");
    }
    
    public void GoToLevel1()
    {
        // Reseta o score para um novo jogo
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scene1");
    }
    
    public void GoToVictory()
    {
        SceneManager.LoadScene("Vitoria");
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