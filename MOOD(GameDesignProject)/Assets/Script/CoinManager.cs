using System.Collections;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public TextMeshProUGUI coinCounterText; // Referência ao TextMeshProUGUI para o contador de moedas
    public TextMeshProUGUI coinMessageText; // Referência ao TextMeshProUGUI para a mensagem de moeda
    public TextMeshProUGUI notEnoughMoneyText; // Referência ao TextMeshProUGUI para a mensagem de "Not Enough Money"
    private int coinCount = 0; // Contador de moedas

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Preservar o CoinManager entre as cenas
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
        StartCoroutine(ShowCoinMessage($"+{amount} Coins"));
    }

    public bool SpendCoins(int amount)
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
            UpdateCoinCounterText();
            StartCoroutine(ShowCoinMessage($"-{amount} Coins"));
            return true;
        }
        else
        {
            StartCoroutine(ShowNotEnoughMoneyMessage("Not Enough Money"));
            return false;
        }
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

    private IEnumerator ShowNotEnoughMoneyMessage(string message)
    {
        notEnoughMoneyText.text = message;
        notEnoughMoneyText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f); // Mostra a mensagem por 2 segundos
        notEnoughMoneyText.gameObject.SetActive(false);
    }
}

