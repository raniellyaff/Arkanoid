using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void OnStartClick()
    {
        GameObject ballObj = GameObject.FindGameObjectWithTag("Ball");
        if (ballObj != null)
        {
            ball ballScript = ballObj.GetComponent<ball>();
            if (ballScript != null)
            {
                ballScript.StartBall();
            }
        }
    }
}