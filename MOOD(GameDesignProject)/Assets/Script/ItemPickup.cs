using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject item_relampagoInRange;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (item_relampagoInRange != null)
            {
                PickUpItem();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("item_relampago"))
        {
            item_relampagoInRange = other.gameObject;
            Debug.Log("Item in range: " + item_relampagoInRange.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("item_relampago"))
        {
            item_relampagoInRange = null;
            Debug.Log("Item out of range.");
        }
    }

    void PickUpItem()
    {
        Debug.Log("Picked up: " + item_relampagoInRange.name);
        Destroy(item_relampagoInRange);
        item_relampagoInRange = null;
    }
}