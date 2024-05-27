using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class soundController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI soundText = null;

    [SerializeField]
    private float maxSliderAmount = 100.0f;

    [SerializeField]
    private AudioSource audioSource = null;
    public void SoundChange(float value)
    {
        float localValue = value * maxSliderAmount;
        soundText.text = localValue.ToString("0");

        audioSource.volume = value;
    }
}
