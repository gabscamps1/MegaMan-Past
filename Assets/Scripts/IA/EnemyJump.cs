using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyJump : MonoBehaviour
{
    public float jumpForce = 5f;
    public float jumpInterval = 2f;
    public Transform player; // Referência ao player, configurada pelo inspetor
    private Rigidbody2D rb;
    private bool isGrounded = true;
    public GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("Jump", jumpInterval, jumpInterval);
        
    }

    void Update()
    {
        target = GameObject.FindWithTag("Player");
        FacePlayer(); // Faz o inimigo olhar na direção do player
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void FacePlayer()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Olha para a direita
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Olha para a esquerda
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
