using UnityEngine;

public class player : MonoBehaviour
{
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;

    public float speed = 12.0f;

    private Rigidbody2D rb2d;
    private float limiteEsquerdo;
    private float limiteDireito;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
        GameObject leftWall = GameObject.Find("LeftWall");
        GameObject rightWall = GameObject.Find("RightWall");
        
        if (leftWall != null && rightWall != null)
        {
            float wallLeftPos = leftWall.transform.position.x;
            float wallRightPos = rightWall.transform.position.x;
            float wallLeftWidth = leftWall.transform.localScale.x;
            float wallRightWidth = rightWall.transform.localScale.x;
            
            float wallLeftEdge = wallLeftPos + (wallLeftWidth / 2);
            float wallRightEdge = wallRightPos - (wallRightWidth / 2);
            
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            float halfWidth = sr != null ? sr.bounds.extents.x : 0.52f;
            
            limiteEsquerdo = wallLeftEdge + halfWidth;
            limiteDireito = wallRightEdge - halfWidth;
        }
        else
        {
            limiteEsquerdo = -4.83f;
            limiteDireito = 4.83f;
        }
        
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        Vector2 vel = rb2d.linearVelocity;

        if (Input.GetKey(moveLeft))
        {
            vel.x = -speed;
        }
        else if (Input.GetKey(moveRight))
        {
            vel.x = speed;
        }
        else
        {
            vel.x = 0;
        }

        rb2d.linearVelocity = vel;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteEsquerdo, limiteDireito);
        transform.position = pos;
    }
}