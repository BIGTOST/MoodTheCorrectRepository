using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animacao : MonoBehaviour
{
    // Referência ao Animator do objeto
    private Animator animator;

    // Referências aos pontos A e B
    public Transform pontoA;
    public Transform pontoB;

    // Velocidade de movimento
    public float speed = 2.0f;

    private Vector3 targetPosition;

    // Start é chamado antes do primeiro frame update
    void Start()
    {
        // Obtém o componente Animator anexado ao objeto
        animator = GetComponent<Animator>();

        // Verifica se o Animator foi encontrado
        if (animator != null)
        {
            // Inicia a animação de correr
            animator.SetBool("isRunning", true); // Define o parâmetro "isRunning" como true para iniciar a animação
        }
        else
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        // Inicialmente, define o alvo como o ponto B
        if (pontoB != null)
        {
            targetPosition = pontoB.position;
        }
        else
        {
            Debug.LogError("PointB is not assigned in the inspector.");
        }
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        if (pontoA == null || pontoB == null)
        {
            Debug.LogError("PointA and PointB need to be assigned in the inspector.");
            return;
        }

        // Move o modelo em direção ao alvo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Verifica se o modelo chegou ao alvo
        if (Vector3.Distance(transform.position, pontoB.position) < 0.1f)
        {
            // Parar a animação de correr
            animator.SetBool("isRunning", false); // Define o parâmetro "isRunning" como false para parar a animação

            // Desabilita este script para parar de mover
            enabled = false;
        }
    }
}
