using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public void OnRestartClick()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.RestartGame();
        }
        else
        {
            // Fallback se não tiver ScoreManager
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
}