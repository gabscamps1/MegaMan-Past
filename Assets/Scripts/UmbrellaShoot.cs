using UnityEngine;

public class EmitParticle : MonoBehaviour
{
    public ParticleSystem particleSystem; // Refer�ncia ao sistema de part�culas
    public float emissionInterval = 2f; // Intervalo de tempo entre emiss�es (definido pelo inspetor)
    public GameObject target; // Refer�ncia ao player (definido pelo inspetor)
    public float detectionRange = 5f; // Dist�ncia m�nima para detectar o player

    private float timeSinceLastEmission;

    void Start()
    {
        timeSinceLastEmission = 0f;
    }

    void Update()
    {
        if (PlayerInRange())
        {
            timeSinceLastEmission += Time.deltaTime;

            if (timeSinceLastEmission >= emissionInterval)
            {
                Emit();
                timeSinceLastEmission = 0f;
            }
        }
    }

    void Emit()
    {
        if (particleSystem != null)
        {
            particleSystem.Emit(1); // Emite uma part�cula
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            target = collision.gameObject;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            target = null;
        }
    }

    bool PlayerInRange()
    {
        if (target == null) return false;

        // Calcula a dist�ncia entre o inimigo e o player
        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

        // Verifica se o player est� dentro da dist�ncia de detec��o
        return distanceToPlayer <= detectionRange;
    }
}
