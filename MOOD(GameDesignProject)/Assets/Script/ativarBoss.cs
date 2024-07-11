using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ativarBoss : MonoBehaviour
{
    public GameObject boss;
    public GameObject spawnLocation;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            Instantiate(boss, new Vector3(spawnLocation.transform.x, spawnLocation.transform.y, spawnLocation.transform.z), Quaternion.identity);
        }
    }
}
