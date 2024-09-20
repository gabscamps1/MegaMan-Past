using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ControlPlayer2 : MonoBehaviour
{
    public Animator anima; // Refer�ncia ao Animator do personagem.
    float xmov; // Vari�vel para guardar o movimento horizontal.
    public Rigidbody2D rdb; // Refer�ncia ao Rigidbody2D do personagem.
    bool jump, sideJump, jumpAgain, jumpLoad, doubleJump, doubleJumpAgain; // Flags para controle de pulo e pulo duplo.
    float jumpTime, jumpTimeSide, jumpTimeLoad, doubleJumpTime; // Controla a dura��o dos pulos.
    public int jumpCount, currentJumps;
    public ParticleSystem fire; // Sistema de part�culas para o efeito de fogo.
    //public Slider jumpBoost;
    void Start()
    {
        // M�todo para inicializa��es. 
        jumpAgain = true;
        jumpCount = 1;
        currentJumps = 0;
    }

    void Update()
    {
        //print(rdb.velocity.y);
        // Captura o movimento horizontal do jogador e Define as velocidades no Animator.
        xmov = Input.GetAxis("Horizontal");
        anima.SetFloat("Velocity", Mathf.Abs(xmov));
        anima.SetFloat("HeightVelocity", Mathf.Abs(rdb.velocity.y));

        // Verifica se o bot�o de pulo foi pressionado, permitindo o pulo lateral.
        if (Input.GetButtonDown("Jump"))
        {
            sideJump = true;
            currentJumps = currentJumps + 1;
        }
            

        if (Input.GetButtonUp("Jump"))
            jumpAgain = true;

        // Define o estado de pulo com base na entrada do usu�rio.
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
        if(doubleJump && Input.GetButtonDown("Jump")) 
        { 
            doubleJumpAgain = true;
            doubleJumpTime = 8;
        }

        // Controla o carregamento do JumpTimeLoad e sua visualiza��o no cen�rio.
        if (Input.GetKey(KeyCode.LeftControl)) // Permite Carregar o JumpTimeLoad.
            jumpLoad = true;
        if (Input.GetKeyUp(KeyCode.LeftControl)) // Pro�be Carregar o JumpTimeLoad.
            jumpLoad = false;
        //jumpBoost.value = jumpTimeLoad / 3; // Configula o valor do JumpBoost no slider proporcional ao valor do JumpTimeLoad.

        // Desativa o estado de "Fire" no Animator.
        anima.SetBool("Fire", false);

        // // Cria a particula de tiro e ativa o estado "Fire" no Animator quando o bot�o de tiro � pressionado.
        if (Input.GetButtonDown("Fire1"))
        {
            anima.SetBool("Fire", true);
            if (!anima.GetBool("Side")) fire.Emit(1); // Condi��es para o player conseguir atirar.
        }

        if (Mathf.Abs(xmov) > 0) Direction(); // Chama a fun��o que inverte o personagem quando o player est� em movimento.
    }

    void FixedUpdate()
    {
        rdb.AddForce(new Vector2(xmov * 20 / (rdb.velocity.magnitude + 1), 0)); // Adiciona uma for�a para mover o personagem.

        // Faz um raycast para baixo para detectar o ch�o para a anima��o de Pulo.
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
        hitright = Physics2D.Raycast(transform.position + (Vector3.up * 0.50f) + (transform.right * 0.45f), transform.right);
        Debug.DrawLine(transform.position + (Vector3.up * 0.50f) + (transform.right * 0.45f), hitright.point);
        if (hitright && hitright.distance < 0.03f && hit.distance > 0.3f)
        {
            JumpRoutineSide(hitright); // Chama a rotina de pulo lateral.
        }
        else
        {
            anima.SetBool("Side", false);
            rdb.gravityScale = 1f; // Volta a velocidade de queda do player para o padr�o quando o player n�o est� segurando na parede.
        }

        JumpLoadRoutine(hit); // Chama a rotina do jump boost.
    }

    // Rotina de jump.
    private void JumpRoutine(RaycastHit2D hit)
    {
        // Verifica a dist�ncia do ch�o e aplica uma for�a de pulo se necess�rio.
        if (hit.distance < 0.1f)
        {
            jumpTime = 1;
            sideJump = false; // Proibe o pulo lateral no JumpRoutineSide() se o player estiver saindo do ch�o.
            doubleJump=false;
            currentJumps = 0;
        }
        else
        {
            if (currentJumps >= jumpCount || anima.GetBool("Side") == true)
                doubleJump = false;
            else if (currentJumps < jumpCount) { 
                 doubleJump = true;
            }
           
        }


        if (jump)
        {
            jumpTime = Mathf.Lerp(jumpTime, 0, Time.fixedDeltaTime * 10);
            rdb.AddForce(Vector2.up * jumpTime, ForceMode2D.Impulse);

            // Impedir de pular enquanto segura a tecla de pulo
            if (rdb.velocity.y < 0)
                jumpAgain = false;
        }


        if (doubleJumpAgain) {
            sideJump = false;
            jumpTime = 0;
            doubleJumpTime = Mathf.Lerp(doubleJumpTime, 0, Time.fixedDeltaTime * 10);
            rdb.AddForce(Vector2.up * doubleJumpTime, ForceMode2D.Impulse);
            doubleJumpAgain = false;
        }

        

    }

    // Rotina de side jump.
    private void JumpRoutineSide(RaycastHit2D hitside)
    {
        if (Mathf.Abs(xmov) > 0) anima.SetBool("Side", true); // Permite a anima��o de Side se o player estiver em movimento olhando para parede.

        // Deixa a velocidade de queda do player mais lenta quando o player desce segurando na parede.
        if (rdb.velocity.y < 0 && Mathf.Abs(xmov) > 0)
            rdb.gravityScale = 0.3f;

        jumpTimeSide = 4.8f;

        // Verifica se est� ocorrendo pode usar o side jump e se a anima��o de side est� ocorrendo e aplica uma for�a de pulo se necess�rio.
        if (sideJump && anima.GetBool("Side") == true)
        {
            rdb.AddForce((hitside.normal + Vector2.up) * jumpTimeSide, ForceMode2D.Impulse);
            sideJump = false; // Impedir de pular de parede em parede segurando o espa�o.
        }
    }

    // Rotina de jump boost.
    private void JumpLoadRoutine(RaycastHit2D hit)
    {
        if (jumpLoad)
        {
            if (hit.distance < 0.1) // Carrega o jumpBoost se o player estiver no ch�o.
            {
                if (jumpTimeLoad < 3) jumpTimeLoad += Time.fixedDeltaTime * 1.5f;
                else jumpTimeLoad = 3;
            }
            else
                jumpTimeLoad = 0; // Descarrega o JumpBoost se o personagem sair do ch�o.
        }
        else
        {
            jumpTimeLoad = Mathf.Lerp(jumpTimeLoad, 0, Time.fixedDeltaTime * 10);
            rdb.AddForce(Vector2.up * jumpTimeLoad, ForceMode2D.Impulse);
        }
    }


    // Fun��o para inverter a rota��o do personagem dependendo da dire��o do movimento.
    void Direction()
    {
        if (rdb.velocity.x > 0.001) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < -0.001) transform.rotation = Quaternion.Euler(0, 180, 0);

    }

    // Detec��o de colis�o com objetos marcados com a tag "Damage" ou "Enemy".
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Damage") || collision.collider.CompareTag("Enemy"))
        {
            LevelManager.instance.LowDamage(); // Chama a fun��o para aplicar dano.
        }
    }

    // Fun��o � chamada pela anima��o de JumpShoot e Walk Shoot.
    private void FrameToFrame()
    {
        float frame = anima.GetCurrentAnimatorStateInfo(0).normalizedTime; // Pega o frame atual da anima��o e normaliza.
        anima.SetFloat("Frames", frame); // Seta o frame atual no novo estado.
    }
}