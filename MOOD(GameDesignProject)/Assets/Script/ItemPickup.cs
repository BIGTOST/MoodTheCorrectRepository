using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject itemInRange;
    
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
        if (other.CompareTag("item"))
        {
            itemInRange = other.gameObject;
            Debug.Log("Item in range: " + itemInRange.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("item"))
        {
            itemInRange = null;
            Debug.Log("Item out of range.");
        }
    }

    void PickUpItem()
    {
        Debug.Log("Picked up: " + itemInRange.name);
        Destroy(itemInRange);
        itemInRange = null;
    }
}
