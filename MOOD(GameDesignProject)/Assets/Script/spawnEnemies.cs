using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemies : MonoBehaviour
{
    private bool passedThrough;

    [SerializeField] private GameObject Inimigo;
    [SerializeField] private GameObject[] enemySpawnPoints;
    [SerializeField] private GameObject player;


    void Awake()
    {
        passedThrough = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        int lucky = Random.Range(1,100),
        spawnChoserd;

        GameObject spawnPosition;

        if(other.tag == "player")
        {
            if(!passedThrough)
            {
                switch(player.getLevel){
                    case 1:
                        if(lucky>= 1 || lucky<=50)
                        {   
                            spawnChoserd = Random.Range(0, enemySpawnPoints.Length);
                            spawnPosition = enemySpawnPoints[spawnChoserd];
                            Instantiate(Inimigo, new Vector3(spawnPosition.trasforms.position.x, 1, spawnPosition.Trasforms.position.z), Quaternion.identity);
                        }
                    break
                    ;
                }
            }
        }
    }
}
