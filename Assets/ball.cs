using UnityEngine;
using UnityEngine.SceneManagement;

public class ball : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool canCollide = true;
    private bool isBallActive = false;
    
    public float ballSpeed = 5f;
    public float minAngle = 25f;
    public float maxAngle = 65f;
    
    private Vector2 startPosition;
    private GameObject startButton;
    private float lastDirectionY = -1f;
    private float lastDirectionX = 0f;

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
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        isBallActive = false;
        
        CancelInvoke();
        ShowStartButton();
    }

    void Update() { }

    public void StartBall()
    {
        if (isBallActive) return;
        
        isBallActive = true;
        HideStartButton();
        GoBall();
    }

    void GoBall()
    {
        float angle = Random.Range(-15f, 15f);
        float rad = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad)).normalized;
        
        if (direction.y > -0.1f)
        {
            direction.y = -1f;
            direction.Normalize();
        }
        
        rb2d.linearVelocity = direction * ballSpeed;
        canCollide = true;
        lastDirectionY = -1f;
        lastDirectionX = direction.x;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!canCollide) return;
        
        // ===== RAQUETE =====
        if (coll.collider.CompareTag("Player"))
        {
            canCollide = false;
            
            float hitPoint = (transform.position.x - coll.transform.position.x) / coll.collider.bounds.size.x;
            float angle = -hitPoint * maxAngle;
            
            if (Mathf.Abs(angle) < minAngle)
            {
                angle = Mathf.Sign(angle) * minAngle;
            }
            if (Mathf.Abs(angle) > maxAngle)
            {
                angle = Mathf.Sign(angle) * maxAngle;
            }
            
            float rad = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)).normalized;
            
            if (direction.y < 0.3f)
            {
                direction.y = 0.5f;
                direction.Normalize();
            }
            
            rb2d.linearVelocity = direction * ballSpeed;
            lastDirectionY = 1f;
            lastDirectionX = direction.x;
            
            Invoke("EnableCollision", 0.1f);
        }
        
        // ===== BLOCOS =====
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
                newVelocity.y = Mathf.Sign(newVelocity.y) * 0.5f;
                newVelocity.x = Mathf.Sign(newVelocity.x) * 0.8f;
                newVelocity.Normalize();
            }
            
            if (Mathf.Abs(newVelocity.y) < 0.3f)
            {
                newVelocity.y = lastDirectionY * 0.5f;
                newVelocity.Normalize();
            }
            
            rb2d.linearVelocity = newVelocity * ballSpeed;
            lastDirectionX = newVelocity.x;
        }
        
        // ===== PAREDES (COLISÃO SIMPLES) =====
        // Parede Esquerda
        if (coll.collider.CompareTag("LeftWall"))
        {
            Vector2 vel = rb2d.linearVelocity;
            vel.x = Mathf.Abs(vel.x); // Força para a direita
            rb2d.linearVelocity = vel.normalized * ballSpeed;
            lastDirectionX = 1f;
        }
        
        // Parede Direita
        if (coll.collider.CompareTag("RightWall"))
        {
            Vector2 vel = rb2d.linearVelocity;
            vel.x = -Mathf.Abs(vel.x); // Força para a esquerda
            rb2d.linearVelocity = vel.normalized * ballSpeed;
            lastDirectionX = -1f;
        }
        
        // Parede Superior
        if (coll.collider.CompareTag("TopWall"))
        {
            Vector2 vel = rb2d.linearVelocity;
            vel.y = -Mathf.Abs(vel.y); // Força para baixo
            rb2d.linearVelocity = vel.normalized * ballSpeed;
            lastDirectionY = -1f;
        }
        
        // ===== CHÃO (PERDE VIDA) =====
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
        lastDirectionY = -1f;
        lastDirectionX = 0f;
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