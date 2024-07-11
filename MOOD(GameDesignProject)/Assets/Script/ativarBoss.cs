using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ativarBoss : MonoBehaviour
{
    public GameObject boss;
    public GameObject spawnLocation;
    public bool isSpawned;
    void Start()
    {
        isSpawned = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!isSpawned){
            if(other.tag == "player")
            {
                Instantiate(boss, new Vector3(spawnLocation.transform.position.x, spawnLocation.transform.position.y, spawnLocation.transform.position.z), Quaternion.identity);
                isSpawned= true;
            }
        }
    }
}
