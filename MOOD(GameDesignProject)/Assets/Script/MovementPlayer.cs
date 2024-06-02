using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementPlayer : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public float meleeRange = 1.5f;
    public float meleeDuration = 0.2f; // Tempo que dura o ataque
    public GameObject meleeColliderPrefab; // Prefab do collider do ataque corpo a corpo
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public Transform firePoint;  // Ponto de onde os proj�teis ser�o disparados

    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;

    public float maxHealth = 100f;
    private float currentHealth;
    public float recuoDistance = 2f; // Dist�ncia do recuo
    public float recuoDuration = 0.5f; // Dura��o do recuo
    private bool isRecoiling = false; // Se o jogador est� recuando
    [SerializeField] 
    private HealthBar _healthbar;
    private Inventory inventory;
    public float itemPickupRange = 2f;

    void Start()
    {
        currentHealth = maxHealth;
        _healthbar.UpdateHealthBar(maxHealth, currentHealth);
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventário não encontrado no jogador.");
        }
    }

    void Update()
    {
        if (!isRecoiling)
        {
            MovePlayer();
            HandleAttacks();
            HandleDash();
            HandleItemPickup();
        }
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
        if (Input.GetButtonDown("Fire1"))  // Bot�o de ataque corpo a corpo (atualmente mouse direito)
        {
            StartCoroutine(MeleeAttack());
        }

        if (Input.GetButtonDown("Fire2"))  // Bot�o de ataque � dist�ncia (atualmente mouse esquerdo)
        {
            RangedAttack();
        }
    }

    IEnumerator MeleeAttack()
    {
        // Criar um collider tempor�rio na frente do jogador para o ataque corpo a corpo
        GameObject meleeCollider = Instantiate(meleeColliderPrefab, transform.position + transform.forward * meleeRange, transform.rotation);
        meleeCollider.tag = "PlayerMeleeAttack"; // Adiciona tag ao collider
        meleeCollider.transform.SetParent(transform);  // Opcional: fazer o collider seguir o jogador

        yield return new WaitForSeconds(meleeDuration);

        Destroy(meleeCollider);
    }

    void RangedAttack()
    {
        // Instanciar e lan�ar o proj�til
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.tag = "PlayerProjectile"; // Adiciona tag ao proj�til
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * projectileSpeed;
        }
        // Falta destruir o objeto ap�s um tempo
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash()); // Ativa o dash caso carregue no espa�o
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
            TakeDamage(10f); // Ajuste o valor do dano conforme necess�rio
        }
    }

    void TakeDamage(float amount)
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
            Die();
        }
    }

    void Die()
    {
        // L�gica de morte do jogador (reiniciar o n�vel, mostrar tela de game over, etc.)
        Debug.Log("O jogador morreu!");
        SceneManager.LoadScene("GameOver");
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
