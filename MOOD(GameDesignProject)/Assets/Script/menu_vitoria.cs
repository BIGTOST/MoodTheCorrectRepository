using UnityEngine;
using TMPro;

public class menu_vitoria : MonoBehaviour
{
    public TMP_Text finalTimeText; // Arraste o componente de texto TMP aqui

    void Start()
    {
        float finalTime = PlayerPrefs.GetFloat("FinalTime");
        DisplayFinalTime(finalTime);
    }

    void DisplayFinalTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        finalTimeText.text = string.Format("You Win!\nTime: {0:00}:{1:00}", minutes, seconds);
    }
}
