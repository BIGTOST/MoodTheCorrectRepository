using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamara : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPosX,
                  currentPosZ;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, currentPosZ), ref velocity, speed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
        currentPosZ = _newRoom.position.z;
    }
}
