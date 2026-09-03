using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PresentationManager : MonoBehaviour
{
    public float displayTime = 2.5f;
    public TextMeshProUGUI levelText;
    public string levelName = "NÍVEL 1";
    
    void Start()
    {
        if (levelText != null)
        {
            levelText.text = levelName;
        }
        
        // Carrega a fase após o tempo
        Invoke("LoadLevel", displayTime);
    }
    
    void LoadLevel()
    {
        SceneManager.LoadScene("Scene1");
    }
}