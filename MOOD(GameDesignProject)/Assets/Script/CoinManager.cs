using System.Collections;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public TextMeshProUGUI coinCounterText; // Referência ao TextMeshProUGUI para o contador de moedas
    public TextMeshProUGUI coinMessageText; // Referência ao TextMeshProUGUI para a mensagem de moeda
    private int coinCount = 0; // Contador de moedas

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        UpdateCoinCounterText();
        StartCoroutine(ShowCoinMessage("+20 Coins"));
    }

    private void UpdateCoinCounterText()
    {
        coinCounterText.text = coinCount.ToString();
    }

    private IEnumerator ShowCoinMessage(string message)
    {
        coinMessageText.text = message;
        coinMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f); // Mostra a mensagem por 2 segundos
        coinMessageText.gameObject.SetActive(false);
    }
}
