using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    public Transform player; // Referência ao transform do jogador
    public MovementPlayer playerData;
    public float attackRange = 2f; // Distância mínima para iniciar o ataque
    public float attackCooldown = 1f; // Tempo de recarga entre ataques
    public float attackDuration = 0.5f; // Duração do collider de ataque
    public GameObject attackColliderPrefab; // Prefab do collider de ataque
    public GameObject coinPrefab; // Prefab da moeda
    public float maxHealth = 100f; // Vida máxima do inimigo
    public float recuoDistance = 2f; // Distância do recuo
    public float recuoDuration = 0.5f; // Duração do recuo

    private float currentHealth; // Vida atual do inimigo
    private float lastAttackTime = 0f; // Armazena o último tempo de ataque
    private NavMeshAgent agent; // Referência ao NavMeshAgent
    private bool isRecoiling = false; // Se o inimigo está recuando
    private Animator anim;
    private float distanceToPlayer;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(anim == null){
            Debug.Log($"Animator not Found");
        }
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

            distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calcula a distância do inimigo ao jogador

            anim.SetBool("Walking", !IsPlayerInRange());
            if (IsPlayerInRange())
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
                    anim.SetTrigger("AttackTrigger");
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    private bool IsPlayerInRange()
    {
        return distanceToPlayer > attackRange;
    }

    void AttackPlayer()
    {
        // Criar um collider temporário para o ataque
        GameObject attackCollider = Instantiate(attackColliderPrefab, transform.position + transform.forward * 1f, transform.rotation);
        attackCollider.tag = "enemyattack";

        // Destruir o collider após um tempo
        Destroy(attackCollider, attackDuration);

        Debug.Log("Atacando o jogador!");
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log("Vida do inimigo: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            playerData.newEnemyKiled();
        }
        else
        {
            StartCoroutine(Recoil());
        }
    }

    void Die()
    {
        // Lógica de morte do inimigo (destruir o objeto, tocar animação, etc.)
        Debug.Log("Inimigo morreu!");

        // Incrementa o contador de moedas ao morrer
        CoinManager.instance.AddCoins(20);

        // Drop de moedas
        DropCoins(4);

        Destroy(gameObject);
    }

void DropCoins(int coinCount)
{
    for (int i = 0; i < coinCount; i++)
    {
        float randomX = Random.Range(0.5f, 1.5f);
        float randomZ = Random.Range(0.5f, 1.5f);
        
        // Ajuste a coordenada y para sempre ser positiva, por exemplo, 1f
        Vector3 randomPosition = transform.position + new Vector3(randomX, 1f, randomZ);
        
        GameObject coin = Instantiate(coinPrefab, randomPosition, Quaternion.identity);
        Debug.Log($"Moeda {i + 1}/{coinCount} dropada na posição {randomPosition}");
        Destroy(coin, 2f); // Destruir a moeda após 2 segundos
    }
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
            // Causa dano ao inimigo quando colidir com o ataque corpo a corpo do jogador
            float meleeDamage = other.gameObject.GetComponentInParent<MovementPlayer>().meleeDamage;
            TakeDamage(meleeDamage);
        }
        else if (other.CompareTag("PlayerProjectile"))
        {
            // Causa dano ao inimigo quando colidir com o projétil do jogador
            float projectileDamage = other.gameObject.GetComponent<Projectile>().damage;
            TakeDamage(projectileDamage);
            Destroy(other.gameObject); // Destroi o projétil ao colidir com o inimigo
        }
    }
}
