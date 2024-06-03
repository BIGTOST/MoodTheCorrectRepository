using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeText : MonoBehaviour
{
    public TMP_Text timeText; // Arraste o componente de texto TMP aqui
    public float startTime;
    private bool gameStarted = false;
    private bool gameEnded = false;

    void Start()
    {
        timeText.text = "00:00";
        StartGame(); // Inicia o jogo quando a cena é carregada
    }

    void Update()
    {
        if (gameStarted && !gameEnded)
        {
            float timeElapsed = Time.time - startTime;
            DisplayTime(timeElapsed);
        }
    }

    public void StartGame()
    {
        startTime = Time.time;
        gameStarted = true;
        gameEnded = false;
    }

    public void EndGame(bool win)
    {
        gameEnded = true;
        float totalTime = Time.time - startTime;
        PlayerPrefs.SetFloat("FinalTime", totalTime);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
