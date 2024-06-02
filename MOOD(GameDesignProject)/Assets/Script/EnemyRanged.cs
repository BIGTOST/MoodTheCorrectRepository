using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyRanged : MonoBehaviour
{
    public Transform player; // Referência ao transform do jogador
    public float attackRange = 10f; // Distância mínima para iniciar o ataque
    public float attackCooldown = 2f; // Tempo de recarga entre ataques
    public GameObject projectilePrefab; // Prefab do projétil
    public Transform firePoint; // Ponto de onde os projéteis serão disparados
    public float projectileSpeed = 15f; // Velocidade do projétil
    public float recuoDistance = 2f; // Distância do recuo ao receber dano
    public float recuoDuration = 0.5f; // Duração do recuo
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isRecoiling = false; // Se o inimigo está recuando
    private NavMeshAgent agent; // Referência ao NavMeshAgent
    private float lastAttackTime = 0f; // Armazena o último tempo de ataque

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Inicializa a referência ao NavMeshAgent
        currentHealth = maxHealth;

        // Verificar se o agente está corretamente configurado
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent não encontrado no inimigo.");
        }
    }

    void Update()
    {
        if (player != null && agent != null && !isRecoiling)
        {
            // Verificar se o agente está ativo e em um NavMesh
            if (!agent.isOnNavMesh)
            {
                Debug.LogError("NavMeshAgent não está no NavMesh.");
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calcula a distância do inimigo ao jogador

            if (distanceToPlayer > attackRange)
            {
                // Move-se em direção ao jogador
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
        // Instanciar e lançar o projétil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = (player.position - firePoint.position).normalized * projectileSpeed;
        }
        Debug.Log("Atacando o jogador com um projétil!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMeleeAttack") || other.CompareTag("PlayerProjectile"))
        {
            TakeDamage(10f); // Ajuste o valor do dano conforme necessário
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
        // Lógica de morte do inimigo (destruir o inimigo, soltar loot, etc.)
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
