using UnityEngine;
using UnityEngine.SceneManagement;

public class ball : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool canCollide = true;
    private bool isBallActive = false;
    
    public float ballSpeed = 8f;
    public float minAngle = 15f;
    public float maxAngle = 70f;
    
    private Vector2 startPosition;
    private GameObject startButton;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        
        rb2d.linearDamping = 0;
        rb2d.angularDamping = 0;
        rb2d.sharedMaterial = null;
        rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        rb2d.gravityScale = 0f;
        rb2d.linearVelocity = Vector2.zero;
        isBallActive = false;
        
        CancelInvoke();
        ShowStartButton();
    }

    void Update() { }

    public void StartBall()
    {
        if (isBallActive) return;
        
        isBallActive = true;
        rb2d.gravityScale = 1f;
        
        HideStartButton();
        GoBall();
    }

    void GoBall()
    {
        Vector2 direction = new Vector2(0, -1).normalized;
        rb2d.linearVelocity = direction * ballSpeed;
        canCollide = true;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!canCollide) return;
        
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
        
        if (coll.collider.CompareTag("LeftWall") || coll.collider.CompareTag("RightWall") || coll.collider.CompareTag("TopWall"))
        {
            if (rb2d.linearVelocity.magnitude > 0)
            {
                rb2d.linearVelocity = rb2d.linearVelocity.normalized * ballSpeed;
            }
        }
        
        if (coll.collider.CompareTag("BottomWall"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.LoseLife();
                
                if (ScoreManager.Instance.GetLives() > 0)
                {
                    ResetBall();
                    ShowStartButton();
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
        rb2d.gravityScale = 0f;
        transform.position = startPosition;
        canCollide = true;
        isBallActive = false;
    }
    
    void ShowStartButton()
    {
        if (startButton == null)
        {
            startButton = GameObject.Find("StartButton");
        }
        
        if (startButton != null)
        {
            startButton.SetActive(true);
        }
    }
    
    void HideStartButton()
    {
        if (startButton == null)
        {
            startButton = GameObject.Find("StartButton");
        }
        
        if (startButton != null)
        {
            startButton.SetActive(false);
        }
    }
    
    public void ResetForNewGame()
    {
        ResetBall();
        ShowStartButton();
    }
}