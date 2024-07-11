using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementPlayer : MonoBehaviour
{


    #region  STATS
    public CharacterController controller;
    public Vector3 startPosition;
    public float speed = 6f;
    public float meleeRange = 1.5f;
    public float meleeDuration = 0.2f; // Tempo que dura o ataque
    public GameObject meleeColliderPrefab; // Prefab do collider do ataque corpo a corpo
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public Transform firePoint;  // Ponto de onde os projéteis serão disparados
    public float projectileLifetime = 5f; // Tempo de vida do projétil
    public float rangedAttackCooldown = 1f; // Tempo de recarga do ataque à distância
    private bool canRangedAttack = true;
    public float meleeDamage = 10f; // Dano do ataque corpo a corpo
    public float projectileDamage = 10f; // Dano do ataque à distância

    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;

    public float maxHealth = 100f;
    public float currentHealth;
    public float recuoDistance = 2f; // Distância do recuo
    public float recuoDuration = 0.5f; // Duração do recuo
    private bool isRecoiling = false; // Se o jogador está recuando

    [SerializeField]
    private HealthBar _healthbar;
    private Inventory inventory;
    public float itemPickupRange = 2f;
    public UIManager uiActions;
    public int enemyKiled;
    public int numeroDeInimigos;
    public GameObejct[] Leveis;
    public int level;
    #endregion
   
    void Start()
    {
        startPosition = transform.position;
        level = 0;
        enemyKiled = 0;
        numeroDeInimigos = 0;
        currentHealth = maxHealth;
        _healthbar.UpdateHealthBar(maxHealth, currentHealth);
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventário não encontrado no jogador.");
        }
    }

    public int getLevel(){
        return this.level;
    }

    private void Update()
    {
        PauseAction();
        EndGame();

        if (!isRecoiling)
        {
            MovePlayer();
            HandleAttacks();
            HandleDash();
            HandleItemPickup();
        }
    }

    public void EndGame()
    {
        if (level == 3)
        {
            //? fazer o unlock da sala do boss
           uiActions.WinGame();

        }
        else{
            level++;
            transform.position = startPosition;
            Leiveis[Level-1].SetActive(false);
            Leveis[level].SetActive(true);
        }
    }

    public void newEnemyKiled()
    {
        enemyKiled++;
    }

    public void updateEnemiesNumbers(int val)
    {
        numeroDeInimigos += val;
    }
    void MovePlayer()
    {
        // Movimentos
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            controller.Move(direction * speed * Time.deltaTime);
        }
    }

    void HandleAttacks()
    {
        if (Input.GetButtonDown("Fire1"))  // Botão de ataque corpo a corpo (atualmente mouse direito)
        {
            StartCoroutine(MeleeAttack());
        }

        if (Input.GetButtonDown("Fire2") && canRangedAttack)  // Botão de ataque à distância (atualmente mouse esquerdo)
        {
            RangedAttack();
        }
    }

    IEnumerator MeleeAttack()
    {
        // Criar um collider temporário na frente do jogador para o ataque corpo a corpo
        GameObject meleeCollider = Instantiate(meleeColliderPrefab, transform.position + transform.forward * meleeRange, transform.rotation);
        meleeCollider.tag = "PlayerMeleeAttack"; // Adiciona tag ao collider
        meleeCollider.transform.SetParent(transform);

        yield return new WaitForSeconds(meleeDuration);

        Destroy(meleeCollider);
    }

    void RangedAttack()
    {
        // Instanciar e lançar o projétil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.tag = "PlayerProjectile"; // Adiciona tag ao projétil
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * projectileSpeed;
        }
        Projectile proj = projectile.AddComponent<Projectile>();
        proj.damage = projectileDamage; // Define o dano do projétil
        StartCoroutine(DestroyProjectileAfterTime(projectile, projectileLifetime));

        canRangedAttack = false;
        StartCoroutine(RangedAttackCooldown());
    }

    IEnumerator DestroyProjectileAfterTime(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (projectile != null)
        {
            Destroy(projectile);
        }
    }

    IEnumerator RangedAttackCooldown()
    {
        yield return new WaitForSeconds(rangedAttackCooldown);
        canRangedAttack = true;
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash()); // Ativa o dash caso carregue no espaço
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            controller.Move(transform.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemyattack"))
        {
            TakeDamage(10f); // Ajuste o valor do dano conforme necessário
        }
    }

    private void PauseAction()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0)
            {
                uiActions.Pause();
            }
            else
            {
                uiActions.UnPause();
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log("Vida do jogador: " + currentHealth);

        _healthbar.UpdateHealthBar(maxHealth, currentHealth);

        if (currentHealth > 0)
        {
            StartCoroutine(Recoil());
        }
        else
        {
            uiActions.GameOver();
            Destroy(this.gameObject);
        }
    }
    
    void Die()
    {
        // Lógica de morte do jogador (reiniciar o nível, mostrar tela de game over, etc.)
        Debug.Log("O jogador morreu!");
        uiActions.GameOver();

    }
    
    public void IncreaseHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        _healthbar.UpdateHealthBar(maxHealth, currentHealth);
        Debug.Log("Health increased. Current health: " + currentHealth);
    }

    public void IncreaseSpeed(float amount)
    {
        speed += amount;
        Debug.Log("Speed increased. Current speed: " + speed);
    }

    public void IncreaseDamage(float amount)
    {
        meleeDamage += amount;
        projectileDamage += amount;
        Debug.Log("Damage increased. Current melee damage: " + meleeDamage + ", projectile damage: " + projectileDamage);
    }

    IEnumerator Recoil()
    {
        isRecoiling = true;
        Vector3 recuoDirection = -transform.forward * recuoDistance;
        float startTime = Time.time;

        while (Time.time < startTime + recuoDuration)
        {
            controller.Move(recuoDirection * Time.deltaTime / recuoDuration);
            yield return null;
        }

        isRecoiling = false;
    }

    void HandleItemPickup()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, itemPickupRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Item"))
                {
                    CollectItem(hitCollider.gameObject);
                }
            }
        }
    }

    void CollectItem(GameObject item)
    {
        inventory.AddItem(item);
        Destroy(item);
    }
}

// Novo script Projectile.cs
public class Projectile : MonoBehaviour
{
    public float damage = 10f; // Define o dano do projétil

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Lógica para causar dano ao inimigo
            EnemyMelee enemy = collision.gameObject.GetComponent<EnemyMelee>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Usa o valor do dano do projétil
            }

            // Destroi o projétil ao colidir com o inimigo
            Destroy(gameObject);
        }
    }
}
