using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class musicController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI musicText = null;

    [SerializeField]
    private float maxSliderAmount = 100.0f;

    [SerializeField]
    private AudioSource audioSource = null;

    public void MusicChange(float value)
    {
        float localValue = value * maxSliderAmount;
        musicText.text = localValue.ToString("0");

        audioSource.volume = value;
    }
}
