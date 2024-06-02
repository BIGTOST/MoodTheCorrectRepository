using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    private float lifetime;
    private string targetTag;

    public void Initialize(float lifetime, string targetTag)
    {
        this.lifetime = lifetime;
        this.targetTag = targetTag;
        Destroy(gameObject, lifetime); // Destrói o projétil após o tempo de vida
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Causa dano ao alvo
            MovementPlayer player = collision.gameObject.GetComponent<MovementPlayer>();
            if (player != null)
            {
                player.TakeDamage(10f); // Ajuste o valor do dano conforme necessário
            }

            // Destrói o projétil ao colidir com o alvo
            Destroy(gameObject);
        }
    }
}
