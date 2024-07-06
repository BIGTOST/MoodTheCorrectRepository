using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public Transform player; // Referência ao transform do jogador
    public float closeAttackRange = 2f; // Distância mínima para o ataque corpo a corpo curto
    public float farAttackRange = 5f; // Distância mínima para o ataque corpo a corpo longo
    public float attackCooldown = 1.5f; // Tempo de recarga entre ataques
    public float closeAttackDuration = 0.5f; // Duração do collider de ataque curto
    public float farAttackDuration = 0.5f; // Duração do collider de ataque longo
    public GameObject closeAttackColliderPrefab; // Prefab do collider de ataque curto
    public GameObject farAttackColliderPrefab; // Prefab do collider de ataque longo
    public float maxHealth = 300f; // Vida máxima do Boss
    public float recuoDistance = 2f; // Distância do recuo
    public float recuoDuration = 0.5f; // Duração do recuo

    private float currentHealth; // Vida atual do Boss
    private float lastAttackTime = 0f; // Armazena o último tempo de ataque
    private NavMeshAgent agent; // Referência ao NavMeshAgent
    private bool isRecoiling = false; // Se o Boss está recuando
    private bool isCloseAttack = true; // Flag para alternar entre ataques

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Inicializa a referência ao NavMeshAgent
        currentHealth = maxHealth;

        // Verificar se o agente está corretamente configurado
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent não encontrado no Boss.");
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

            float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calcula a distância do Boss ao jogador

            if (distanceToPlayer > farAttackRange)
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
                    if (isCloseAttack && distanceToPlayer <= closeAttackRange)
                    {
                        StartCoroutine(PerformCloseAttack());
                    }
                    else if (!isCloseAttack && distanceToPlayer <= farAttackRange)
                    {
                        StartCoroutine(PerformFarAttack());
                    }

                    lastAttackTime = Time.time;
                    isCloseAttack = !isCloseAttack; // Alterna o ataque para a próxima vez
                }
            }
        }
    }

    IEnumerator PerformCloseAttack()
    {
        // Criar um collider temporário para o ataque curto
        Vector3 attackPosition = transform.position + transform.forward * (closeAttackRange / 2);
        GameObject attackCollider = Instantiate(closeAttackColliderPrefab, attackPosition, transform.rotation);
        attackCollider.tag = "enemyattack"; // Adiciona tag ao collider

        // Dimensionar o collider com base no alcance do ataque
        BoxCollider boxCollider = attackCollider.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(1f, 1f, closeAttackRange);
        }

        yield return new WaitForSeconds(closeAttackDuration);

        Destroy(attackCollider);
    }

    IEnumerator PerformFarAttack()
    {
        // Criar um collider temporário para o ataque longo
        Vector3 attackPosition = transform.position + transform.forward * (farAttackRange / 2);
        GameObject attackCollider = Instantiate(farAttackColliderPrefab, attackPosition, transform.rotation);
        attackCollider.tag = "enemyattack"; // Adiciona tag ao collider

        // Dimensionar o collider com base no alcance do ataque
        BoxCollider boxCollider = attackCollider.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(1f, 1f, farAttackRange);
        }

        yield return new WaitForSeconds(farAttackDuration);

        Destroy(attackCollider);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log("Vida do Boss: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Recoil());
        }
    }

    void Die()
    {
        // Lógica de morte do Boss (destruir o objeto, tocar animação, etc.)
        Debug.Log("Boss morreu!");
        Destroy(gameObject);
    }

    IEnumerator Recoil()
    {
        isRecoiling = true;
        Vector3 recuoDirection = -transform.forward * recuoDistance;
        Vector3 recuoTarget = transform.position + recuoDirection;

        float startTime = Time.time;
        while (Time.time < startTime + recuoDuration)
        {
            agent.Move(recuoDirection * Time.deltaTime / recuoDuration);
            yield return null;
        }

        isRecoiling = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMeleeAttack"))
        {
            TakeDamage(20f); // Ajuste o valor do dano conforme necessário
        }
        else if (other.CompareTag("PlayerProjectile"))
        {
            TakeDamage(10f); // Ajuste o valor do dano conforme necessário
            Destroy(other.gameObject); // Destrói o projétil ao colidir
        }
    }
}
