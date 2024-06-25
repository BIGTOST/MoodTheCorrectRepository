    using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    public Transform player; // Refer�ncia ao transform do jogador
    public MovementPlayer playerData;
    public float attackRange = 2f; // Dist�ncia m�nima para iniciar o ataque
    public float attackCooldown = 1f; // Tempo de recarga entre ataques
    public float attackDuration = 0.5f; // Dura��o do collider de ataque
    public GameObject attackColliderPrefab; // Prefab do collider de ataque
    public float maxHealth = 100f; // Vida m�xima do inimigo
    public float recuoDistance = 2f; // Dist�ncia do recuo
    public float recuoDuration = 0.5f; // Dura��o do recuo

    private float currentHealth; // Vida atual do inimigo
    private float lastAttackTime = 0f; // Armazena o �ltimo tempo de ataque
    private NavMeshAgent agent; // Refer�ncia ao NavMeshAgent
    private bool isRecoiling = false; // Se o inimigo est� recuando
    private Animator anim;
    private float distanceToPlayer;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(anim == null){
            Debug.Log($"Animator not Found");
        }
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

            distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calcula a dist�ncia do inimigo ao jogador

            anim.SetBool("Walking", !IsPlayerInRange());
            if (IsPlayerInRange())
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
                    anim.SetTrigger("AttackTrigger");
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    private bool IsPlayerInRange(){
        return distanceToPlayer > attackRange;
    }
    void AttackPlayer()
    {
        
        
        // Criar um collider tempor�rio para o ataque
        GameObject attackCollider = Instantiate(attackColliderPrefab, transform.position + transform.forward * 1f, transform.rotation);
        attackCollider.tag = "enemyattack";

        // Destruir o collider ap�s um tempo
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
        // L�gica de morte do inimigo (destruir o objeto, tocar anima��o, etc.)
        Debug.Log("Inimigo morreu!");
        Destroy(gameObject);
    }

    IEnumerator Recoil(){
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
            TakeDamage(20f); // Ajuste o valor do dano conforme necess�rio
        }
        else if (other.CompareTag("PlayerProjectile"))
        {
            TakeDamage(10f); // Ajuste o valor do dano conforme necess�rio
            Destroy(other.gameObject); // Destroi o proj�til ao colidir
        }
    }
}
