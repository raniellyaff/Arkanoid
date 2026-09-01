using UnityEngine;

public class player : MonoBehaviour
{
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;

    public float speed = 10.0f;

    // Posição das paredes
    private float wallLeft = -8.05f;
    private float wallRight = 8.05f;

    // Metade da largura da raquete
    private float halfWidth = 0.52f;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 vel = rb2d.linearVelocity;

        // Movimento para a esquerda
        if (Input.GetKey(moveLeft))
        {
            vel.x = -speed;
        }
        // Movimento para a direita
        else if (Input.GetKey(moveRight))
        {
            vel.x = speed;
        }
        // Para a raquete
        else
        {
            vel.x = 0;
        }

        rb2d.linearVelocity = vel;

        // Posição atual
        Vector3 pos = transform.position;

        // Limites considerando o tamanho da raquete
        float limiteEsquerdo = wallLeft + halfWidth;
        float limiteDireito = wallRight - halfWidth;

        // Impede a raquete de atravessar as paredes
        pos.x = Mathf.Clamp(pos.x, limiteEsquerdo, limiteDireito);

        transform.position = pos;
    }
}
