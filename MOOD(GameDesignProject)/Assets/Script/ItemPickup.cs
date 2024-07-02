using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    // SerializeField para associar os TextMesh Pro no Unity
    [SerializeField] private TextMeshProUGUI agilityText;
    [SerializeField] private TextMeshProUGUI powerText;

    // Contadores
    private int agilityCount = 0;
    private int powerCount = 0;

    private GameObject itemInRange;

    void Start()
    {
        // Inicializa os textos no início
        UpdateAgilityText();
        UpdatePowerText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (itemInRange != null)
            {
                PickUpItem();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("item_relampago") || other.CompareTag("item_escudo") || other.CompareTag("item_amuleto"))
        {
            itemInRange = other.gameObject;
            Debug.Log("Item in range: " + itemInRange.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("item_relampago") || other.CompareTag("item_escudo") || other.CompareTag("item_amuleto"))
        {
            itemInRange = null;
            Debug.Log("Item out of range.");
        }
    }

    void PickUpItem()
    {
        Debug.Log("Picked up: " + itemInRange.name);

        if (itemInRange.CompareTag("item_relampago"))
        {
            agilityCount += 10;
            UpdateAgilityText();
        }
        else if (itemInRange.CompareTag("item_escudo") || itemInRange.CompareTag("item_amuleto"))
        {
            powerCount += 10;
            UpdatePowerText();
        }

        Destroy(itemInRange);
        itemInRange = null;
    }

    // Método para atualizar o TextMesh Pro com o valor atual do contador de agilidade
    void UpdateAgilityText()
    {
        agilityText.text = agilityCount.ToString();
        Debug.Log("Updated Agility Text: " + agilityText.text);
    }

    // Método para atualizar o TextMesh Pro com o valor atual do contador de poder
    void UpdatePowerText()
    {
        powerText.text = powerCount.ToString();
        Debug.Log("Updated Power Text: " + powerText.text);
    }
}
