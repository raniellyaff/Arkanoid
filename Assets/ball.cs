using UnityEngine;
using UnityEngine.SceneManagement;

public class ball : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float ballSpeed = 10f;

    void GoBall()
    {
        float rand = Random.Range(0, 2);

        if (rand < 1)
        {
            rb2d.linearVelocity = new Vector2(-0.7f, 1f).normalized * ballSpeed;
        }
        else
        {
            rb2d.linearVelocity = new Vector2(0.7f, 1f).normalized * ballSpeed;
        }
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        Invoke("GoBall", 2);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // Colisão com a raquete
        if (coll.collider.CompareTag("Player"))
        {
            // Verifica em qual lado da raquete a bola bateu
            float hitPoint = transform.position.x - coll.transform.position.x;

            // Define se a bola vai para a esquerda ou direita
            float directionX;

            if (hitPoint < 0)
            {
                directionX = -0.7f;
            }
            else
            {
                directionX = 0.7f;
            }

            // A bola sempre sobe ao bater na raquete
            Vector2 direction = new Vector2(directionX, 1f).normalized;

            rb2d.linearVelocity = direction * ballSpeed;
        }

        // Impede que a bola fique andando somente para os lados
        Vector2 velocity = rb2d.linearVelocity;

        if (Mathf.Abs(velocity.y) < 3f)
        {
            velocity.y = velocity.y >= 0 ? 3f : -3f;
            rb2d.linearVelocity = velocity.normalized * ballSpeed;
        }

        // Se a bola cair na BottomWall, reinicia o jogo
        if (coll.collider.CompareTag("BottomWall"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Colisão com os blocos
        if (coll.collider.CompareTag("Brick"))
        {
            Destroy(coll.gameObject);
        }
    }

    // Reinicializa a posição e velocidade da bola
    void ResetBall()
    {
        rb2d.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    // Reinicializa o jogo
    void RestartGame()
    {
        ResetBall();
        Invoke("GoBall", 1);
    }
}
