using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAction : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private MoveCamara cam;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other}");
        if(other.tag == "player")
        {
            
            if(other.transform.position.x < transform.position.x || other.transform.position.z < transform.position.z)
            {
                cam.MoveToNewRoom(nextRoom);
            }
            else if(other.transform.position.x > transform.position.x || other.transform.position.z > transform.position.z)
            {
                cam.MoveToNewRoom(previousRoom);
            }
        }
    }
}