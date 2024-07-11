using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public GameObject player; // Refer�ncia ao transform do jogador
    public MovementPlayer playerData;
    public float closeAttackRange = 2f; // Dist�ncia m�nima para o ataque corpo a corpo curto
    public float farAttackRange = 5f; // Dist�ncia m�nima para o ataque corpo a corpo longo
    public float attackCooldown = 1.5f; // Tempo de recarga entre ataques
    public float closeAttackDuration = 0.5f; // Dura��o do collider de ataque curto
    public float farAttackDuration = 0.5f; // Dura��o do collider de ataque longo
    public GameObject closeAttackColliderPrefab; // Prefab do collider de ataque curto
    public GameObject farAttackColliderPrefab; // Prefab do collider de ataque longo
    public float maxHealth = 10F; // Vida m�xima do Boss

    public float currentHealth; // Vida atual do Boss
    private float lastAttackTime = 0f; // Armazena o �ltimo tempo de ataque
    private NavMeshAgent agent; // Refer�ncia ao NavMeshAgent
    private bool isCloseAttack = true; // Flag para alternar entre ataques

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("player")[0];
        playerData = player.GetComponent<MovementPlayer>();
        agent = GetComponent<NavMeshAgent>(); // Inicializa a refer�ncia ao NavMeshAgent
        currentHealth = maxHealth;

        // Verificar se o agente est� corretamente configurado
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent n�o encontrado no Boss.");
        }
    }

    void Update()
    {
        if (player.transform != null && agent != null)
        {
            // Verificar se o agente est� ativo e em um NavMesh
            if (!agent.isOnNavMesh)
            {
                Debug.LogError("NavMeshAgent n�o est� no NavMesh.");
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position); // Calcula a dist�ncia do Boss ao jogador

            if (distanceToPlayer > farAttackRange)
            {
                // Move-se em dire��o ao jogador
                agent.SetDestination(player.transform.position);
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
                    isCloseAttack = !isCloseAttack; // Alterna o ataque para a pr�xima vez
                }
            }
        }
    }

    IEnumerator PerformCloseAttack()
    {
        // Criar um collider tempor�rio para o ataque curto
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
        // Criar um collider tempor�rio para o ataque longo
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
            playerData.EndGame();
        }
    }

    void Die()
    {
        // L�gica de morte do Boss (destruir o objeto, tocar anima��o, etc.)
        Debug.Log("Boss morreu!");
        Destroy(gameObject);

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Algo Acertou");
        Debug.Log($"{other.tag}");
        if (other.tag=="player")
        {
            // Causa dano ao inimigo quando colidir com o ataque corpo a corpo do jogador
            float meleeDamage = playerData.meleeDamage;
                Debug.Log($"Tomo damage");
                TakeDamage(meleeDamage);
        }
        else if (other.tag=="PlayerProjectile")
        {
            // Causa dano ao inimigo quando colidir com o proj�til do jogador
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                float projectileDamage = projectile.damage;
                TakeDamage(projectileDamage);
                Destroy(other.gameObject); // Destroi o proj�til ao colidir com o inimigo
            }
        }
    }
}
