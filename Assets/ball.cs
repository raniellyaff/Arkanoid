using UnityEngine;

public class ball : MonoBehaviour
{
    private Rigidbody2D rb2d;

    // Inicializa a bola aleatoriamente para esquerda ou direita
    void GoBall()
    {
        float rand = Random.Range(0, 2);

        if (rand < 1)
        {
            rb2d.AddForce(new Vector2(20, -15));
        }
        else
        {
            rb2d.AddForce(new Vector2(-20, -15));
        }
    }

    void Start()
    {
        // Inicializa o objeto bola
        rb2d = GetComponent<Rigidbody2D>();

        // Chama a função GoBall após 2 segundos
        Invoke("GoBall", 2);
    }

    // Determina o comportamento da bola nas colisões com os Players
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            Vector2 vel;

            vel.x = rb2d.linearVelocity.x;
            vel.y = (rb2d.linearVelocity.y / 2) +
                    (coll.collider.attachedRigidbody.linearVelocity.y / 3);

            rb2d.linearVelocity = vel;
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
