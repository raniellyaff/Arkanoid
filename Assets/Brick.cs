using UnityEngine;

public class Brick : MonoBehaviour
{
    public int points = 10;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(points);
            }
            
            Destroy(gameObject);
        }
    }
}