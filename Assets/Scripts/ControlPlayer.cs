using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    public Animator anima; // Referência ao Animator do personagem.
    float xmov; // Variável para guardar o movimento horizontal.
    public Rigidbody2D rdb; // Referência ao Rigidbody2D do personagem.
    bool jump, sideJump, jumpAgain; // Flags para controle de pulo e pulo duplo.
    float jumpTime, jumpTimeSide; // Controla a duração dos pulos.
    public ParticleSystem fire; // Sistema de partículas para o efeito de fogo.
    void Start()
    {
        // Método para inicializações. 
        jumpAgain = true;
    }

    void Update()
    {
        // Captura o movimento horizontal do jogador e Define as velocidades no Animator.
        xmov = Input.GetAxis("Horizontal");
        anima.SetFloat("Velocity", Mathf.Abs(xmov));
        anima.SetFloat("HeightVelocity", Mathf.Abs(rdb.velocity.y));

        // Verifica se o botão de pulo foi pressionado, permitindo o pulo lateral.
        if (Input.GetButtonDown("Jump"))
            sideJump = true;

        if (Input.GetButtonUp("Jump"))
            jumpAgain = true;

        // Define o estado de pulo com base na entrada do usuário.
        if (Input.GetButton("Jump") && jumpAgain)
        {
            jump = true;
        }
        else
        {
            jump = false;
            jumpTime = 0;
            jumpTimeSide = 0;
        }

        // Desativa o estado de "Fire" no Animator.
        anima.SetBool("Fire", false);

        // Chama a função Tiro
        if (Input.GetButtonDown("Fire1"))
        {
            anima.SetBool("Fire", true); // ativa o estado "Fire" no Animator quando o botão de tiro é pressionado.
            fire.Emit(1);
        }


        if (Mathf.Abs(xmov) > 0) Direction(); // Chama a função que inverte o personagem quando o player está em movimento.
    }

    void FixedUpdate()
    {
        rdb.AddForce(new Vector2(xmov * 20 / (rdb.velocity.magnitude + 1), 0)); // Adiciona uma força para mover o personagem.

        // Faz um raycast para baixo para detectar o chão para a animação de Pulo.
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position + Vector3.down * 0.5f, Vector2.down);
        Debug.DrawLine(transform.position + Vector3.down * 0.5f, hit.point);
        if (hit)
        {
            anima.SetFloat("Height", hit.distance);
            JumpRoutine(hit); // Chama a rotina de pulo.
        }

        // Faz um raycast para a direita para detectar paredes.
        RaycastHit2D hitright;
        hitright = Physics2D.Raycast(transform.position + (Vector3.up * 0.41f) + (transform.right * 0.45f), transform.right);
        Debug.DrawLine(transform.position + (Vector3.up * 0.41f) + (transform.right * 0.45f), hitright.point);
        if (hitright && hitright.distance < 0.03f && hit.distance > 0.3f)
        {
            JumpRoutineSide(hitright); // Chama a rotina de pulo lateral.
        }
        else
        {
            anima.SetBool("Side", false);
            rdb.gravityScale = 1f; // Volta a velocidade de queda do player para o padrão quando o player não está segurando na parede.
        }

    }

    // Rotina de pulo (parte física).
    private void JumpRoutine(RaycastHit2D hit)
    {
        // Verifica a distância do chão e aplica uma força de pulo se necessário.
        if (hit.distance < 0.1f)
        {
            jumpTime = 1;
            sideJump = false; // Proibe o pulo lateral se o player estiver saindo do chão.
        }

        if (jump)
        {
            jumpTime = Mathf.Lerp(jumpTime, 0, Time.fixedDeltaTime * 10);
            rdb.AddForce(Vector2.up * jumpTime, ForceMode2D.Impulse);

            // Impedir de pular enquanto segura a tecla de pulo
            if (rdb.velocity.y < 0)
                jumpAgain = false;
        }

    }

    // Rotina de pulo lateral.
    private void JumpRoutineSide(RaycastHit2D hitside)
    {
        anima.SetBool("Side", true);

        // Deixa a velocidade de queda do player mais lenta quando o player desce segurando na parede.
        if (rdb.velocity.y < 0 && Mathf.Abs(xmov) > 0)
            rdb.gravityScale = 0.3f;

        jumpTimeSide = 6;

        if (sideJump) //CORRIJIR DEPOIS
        {
            jumpTimeSide = Mathf.Lerp(jumpTimeSide, 0, Time.fixedDeltaTime * 10);
            rdb.AddForce((hitside.normal + Vector2.up) * jumpTimeSide, ForceMode2D.Impulse);
            sideJump = false;
        }
    }


    // Função para inverter a rotação do personagem dependendo da direção do movimento.
    void Direction()
    {
        if (rdb.velocity.x > 0.001) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < -0.001) transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Função para inverter a rotação do personagem de forma forçada.
    void PhisicalReverser()
    {
        transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + 180) % 360, 0);
    }

    // Detecção de colisão com objetos marcados com a tag "Damage" ou "Enemy".
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Damage") || collision.collider.CompareTag("Enemy"))
        {
            LevelManager.instance.LowDamage(); // Chama a função para aplicar dano.
        }
    }

    // Função é chamada pela animação de JumpShoot e Walk Shoot.
    private void FrameToFrame()
    {
        float frame = anima.GetCurrentAnimatorStateInfo(0).normalizedTime; // Pega o frame atual da animação e normaliza.
        anima.SetFloat("Frames", frame); // Seta o frame atual no novo estado.
    }
}

