using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI agilityText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private MovementPlayer player; // Referência ao script do jogador

    private int agilityCount = 0;
    private int powerCount = 0;
    private GameObject itemInRange;

    void Start()
    {
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
        if (other.CompareTag("item_relampago") || other.CompareTag("item_escudo") || other.CompareTag("item_amuleto") || other.CompareTag("item_orbe"))
        {
            itemInRange = other.gameObject;
            Debug.Log("Item in range: " + itemInRange.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("item_relampago") || other.CompareTag("item_escudo") || other.CompareTag("item_amuleto") || other.CompareTag("item_orbe"))
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
        else if (itemInRange.CompareTag("item_orbe"))
        {
            Debug.Log("Orbe item picked up, increasing player health.");
            player.IncreaseHealth(10f); // Aumenta a saúde do jogador
        }

        Destroy(itemInRange);
        itemInRange = null;
    }

    void UpdateAgilityText()
    {
        agilityText.text = agilityCount.ToString();
        Debug.Log("Updated Agility Text: " + agilityText.text);
    }

    void UpdatePowerText()
    {
        powerText.text = powerCount.ToString();
        Debug.Log("Updated Power Text: " + powerText.text);
    }
}
