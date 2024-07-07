using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyRanged : MonoBehaviour
{
    public Transform player; // Refer�ncia ao transform do jogador
    public float attackRange = 10f; // Dist�ncia m�nima para iniciar o ataque
    public float attackCooldown = 2f; // Tempo de recarga entre ataques
    public GameObject projectilePrefab; // Prefab do proj�til
    public Transform firePoint; // Ponto de onde os proj�teis ser�o disparados
    public float projectileSpeed = 15f; // Velocidade do proj�til
    public float recuoDistance = 2f; // Dist�ncia do recuo ao receber dano
    public float recuoDuration = 0.5f; // Dura��o do recuo
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isRecoiling = false; // Se o inimigo est� recuando
    private NavMeshAgent agent; // Refer�ncia ao NavMeshAgent
    private float lastAttackTime = 0f; // Armazena o �ltimo tempo de ataque
    public float projectileLifetime = 5f; // Tempo de vida do proj�til

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Inicializa a refer�ncia ao NavMeshAgent
        currentHealth = maxHealth;

        // Verificar se o agente est� corretamente configurado
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent n�o encontrado no inimigo.");
        }
    }

    void Update()
    {
        if (player != null && agent != null && !isRecoiling)
        {
            // Verificar se o agente est� ativo e em um NavMesh
            if (!agent.isOnNavMesh)
            {
                Debug.LogError("NavMeshAgent n�o est� no NavMesh.");
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calcula a dist�ncia do inimigo ao jogador

            if (distanceToPlayer > attackRange)
            {
                // Move-se em dire��o ao jogador
                agent.SetDestination(player.position);
            }
            else
            {
                // Para de se mover e ataca
                agent.SetDestination(transform.position);

                if (Time.time > lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    void AttackPlayer()
    {
        // Instanciar e lan�ar o proj�til
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = (player.position - firePoint.position).normalized * projectileSpeed;
        }
        Debug.Log("Atacando o jogador com um proj�til!");
        projectile.AddComponent<ProjectileHandler>().Initialize(projectileLifetime, "Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMeleeAttack") || other.CompareTag("PlayerProjectile"))
        {
            TakeDamage(10f); // Ajuste o valor do dano conforme necess�rio
        }
    }

    void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log("Vida do inimigo: " + currentHealth);

        if (currentHealth > 0)
        {
            StartCoroutine(Recoil());
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        // L�gica de morte do inimigo (destruir o inimigo, soltar loot, etc.)
        Debug.Log("O inimigo morreu!");
        Destroy(gameObject);
    }

    IEnumerator Recoil()
    {
        isRecoiling = true;
        Vector3 recuoDirection = -transform.forward * recuoDistance;
        float startTime = Time.time;

        while (Time.time < startTime + recuoDuration)
        {
            agent.Move(recuoDirection * Time.deltaTime / recuoDuration);
            yield return null;
        }

        isRecoiling = false;
    }
}
