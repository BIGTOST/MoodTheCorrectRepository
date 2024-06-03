using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private GameObject gameOverUI; // Arraste o painel de Game Over aqui no Inspetor

    // Atualiza a barra de saúde
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _healthbarSprite.fillAmount = currentHealth / maxHealth;

        // Verifica se a saúde chegou a zero
        if (currentHealth <= 0)
        {
            ShowGameOverUI();
        }
    }

    // Mostra o painel de Game Over
    private void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }
}