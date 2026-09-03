using UnityEngine;
using UnityEngine.SceneManagement;

public class ball : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool canCollide = true;
    
    public float ballSpeed = 8f;
    public float minAngle = 20f;
    public float maxAngle = 70f;
    
    private Vector2 startPosition;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        
        rb2d.gravityScale = 1f;
        rb2d.linearDamping = 0;
        rb2d.angularDamping = 0;
        rb2d.sharedMaterial = null;
        rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        Invoke("GoBall", 2);
    }

    void GoBall()
    {
        float angle = Random.Range(minAngle, maxAngle);
        
        if (Random.Range(0, 2) == 0)
        {
            angle = -angle;
        }
        
        float rad = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)).normalized;
        
        rb2d.linearVelocity = direction * ballSpeed;
        canCollide = true;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!canCollide) return;
        
        // RAQUETE
        if (coll.collider.CompareTag("Player"))
        {
            canCollide = false;
            
            float hitPoint = (transform.position.x - coll.transform.position.x) / coll.collider.bounds.size.x;
            float angle = -hitPoint * maxAngle;
            
            if (Mathf.Abs(angle) < minAngle)
            {
                angle = Mathf.Sign(angle) * minAngle;
            }
            
            float rad = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)).normalized;
            
            rb2d.linearVelocity = direction * ballSpeed;
            
            Invoke("EnableCollision", 0.1f);
        }
        
        // BLOCOS
        if (coll.gameObject.CompareTag("Brick"))
        {
            Brick brickScript = coll.gameObject.GetComponent<Brick>();
            if (brickScript != null && ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(brickScript.points);
            }
            
            Destroy(coll.gameObject);
            
            Vector2 normal = coll.contacts[0].normal;
            Vector2 newVelocity = Vector2.Reflect(rb2d.linearVelocity.normalized, normal);
            
            float angleDeg = Mathf.Atan2(Mathf.Abs(newVelocity.y), Mathf.Abs(newVelocity.x)) * Mathf.Rad2Deg;
            if (angleDeg < minAngle)
            {
                newVelocity.y = Mathf.Sign(newVelocity.y) * 0.3f;
                newVelocity.Normalize();
            }
            
            rb2d.linearVelocity = newVelocity * ballSpeed;
        }
        
        // PAREDES
        if (coll.collider.CompareTag("LeftWall") || coll.collider.CompareTag("RightWall") || coll.collider.CompareTag("TopWall"))
        {
            if (rb2d.linearVelocity.magnitude > 0)
            {
                rb2d.linearVelocity = rb2d.linearVelocity.normalized * ballSpeed;
            }
        }
        
        // CHÃO - PERDE UMA VIDA
        if (coll.collider.CompareTag("BottomWall"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.LoseLife();
                
                if (ScoreManager.Instance.GetLives() > 0)
                {
                    ResetBall();
                    Invoke("GoBall", 1.5f);
                }
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void EnableCollision()
    {
        canCollide = true;
    }

    void ResetBall()
    {
        rb2d.linearVelocity = Vector2.zero;
        transform.position = startPosition;
        canCollide = true;
    }
}