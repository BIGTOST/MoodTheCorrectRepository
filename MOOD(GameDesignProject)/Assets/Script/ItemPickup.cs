using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI agilityText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI pickupMessageText; 
    [SerializeField] private MovementPlayer player;

    private int agilityCount = 0;
    private int powerCount = 0;
    private GameObject itemInRange;

    void Start()
    {
        UpdateAgilityText();
        UpdatePowerText();
        pickupMessageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (itemInRange != null)
            {
                PickUpItem();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("item_relampago") || other.CompareTag("item_escudo") || other.CompareTag("item_amuleto") || other.CompareTag("item_orbe") ||
            other.CompareTag("loja_relampago") || other.CompareTag("loja_escudo") || other.CompareTag("loja_amuleto") || other.CompareTag("loja_orbe"))
        {
            itemInRange = other.gameObject;
            Debug.Log("Item in range: " + itemInRange.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("item_relampago") || other.CompareTag("item_escudo") || other.CompareTag("item_amuleto") || other.CompareTag("item_orbe") ||
            other.CompareTag("loja_relampago") || other.CompareTag("loja_escudo") || other.CompareTag("loja_amuleto") || other.CompareTag("loja_orbe"))
        {
            itemInRange = null;
            Debug.Log("Item out of range.");
        }
    }

    void PickUpItem()
    {
        Debug.Log("Picked up: " + itemInRange.name);

        if (itemInRange.CompareTag("item_relampago") || itemInRange.CompareTag("loja_relampago"))
        {
            agilityCount += 10;
            UpdateAgilityText();
            StartCoroutine(ShowPickupMessage("+10 Agility")); 
        }
        else if (itemInRange.CompareTag("item_escudo") || itemInRange.CompareTag("loja_escudo") ||
                 itemInRange.CompareTag("item_amuleto") || itemInRange.CompareTag("loja_amuleto"))
        {
            powerCount += 10;
            UpdatePowerText();
            StartCoroutine(ShowPickupMessage("+10 Power")); 
        }
        else if (itemInRange.CompareTag("item_orbe") || itemInRange.CompareTag("loja_orbe"))
        {
            Debug.Log("Orbe item picked up, increasing player health.");
            player.IncreaseHealth(10f); 
            StartCoroutine(ShowPickupMessage("+10 Health")); 
        }

        if (itemInRange.CompareTag("item_relampago") || itemInRange.CompareTag("item_escudo") ||
            itemInRange.CompareTag("item_amuleto") || itemInRange.CompareTag("item_orbe"))
        {
            Destroy(itemInRange);
        }
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

    private IEnumerator ShowPickupMessage(string message)
    {
        pickupMessageText.text = message;
        pickupMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f); 
        pickupMessageText.gameObject.SetActive(false);
    }
}
