using UnityEngine;

public class YokaiUmbrellaAI : MonoBehaviour
{
    public float speed = 2f;
    public Transform groundCheck; // Ponto para verificar o ch�o
    public float groundCheckDistance = 1f; // Dist�ncia para verificar o ch�o
    public Transform wallCheck; // Ponto para verificar colis�es com parede
    public float wallCheckDistance = 0.1f; // Dist�ncia para verificar a parede
    public LayerMask groundLayer; // Definir o que � considerado ch�o e parede
    private bool movingRight = true;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimento do inimigo
        rb.velocity = new Vector2(speed * (movingRight ? 1 : -1), rb.velocity.y);

        // Verifica se h� ch�o � frente
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        if (!groundInfo.collider)
        {
            Flip(); // Muda de dire��o se n�o houver ch�o
        }

        // Verifica se h� uma parede � frente
        RaycastHit2D wallInfo = Physics2D.Raycast(wallCheck.position, movingRight ? Vector2.right : Vector2.left, wallCheckDistance, groundLayer);
        if (wallInfo.collider)
        {
            Flip(); // Muda de dire��o se colidir com uma parede
        }
    }

    void Flip()
    {
        movingRight = !movingRight;

        // Inverte a escala no eixo X
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnDrawGizmosSelected()
    {
        // Visualizar o groundCheck e o wallCheck no Editor
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (movingRight ? Vector3.right : Vector3.left) * wallCheckDistance);
    }
}
