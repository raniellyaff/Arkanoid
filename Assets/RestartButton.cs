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
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene1");
        }
    }
}