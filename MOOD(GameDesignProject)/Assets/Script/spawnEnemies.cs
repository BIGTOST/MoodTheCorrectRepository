using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemies : MonoBehaviour
{
    private bool passedThrough;

    [SerializeField] public GameObject Inimigo;
    [SerializeField] public GameObject[] enemySpawnPoints;
    [SerializeField] public MovementPlayer player;


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
                switch(player.getLevel()){
                    case 1:

                        //50% de chande de dar spawn ao inimigo
                        if(lucky>= 1 || lucky<=50)
                        {   
                            spawnChoserd = Random.Range(0, enemySpawnPoints.Length);
                            spawnPosition = enemySpawnPoints[spawnChoserd];
                            Instantiate(Inimigo, new Vector3(spawnPosition.transform.position.x, spawnPosition.transform.position.y, spawnPosition.transform.position.z), Quaternion.identity);
                        }
                        //*40% Chance de dar spawna a 2 inimigos
                        else if(lucky >=51 || lucky <= 90){
                            for(int i = 0; i < 2 ; i++) {
                                spawnChoserd = Random.Range(0, enemySpawnPoints.Length);
                                spawnPosition = enemySpawnPoints[spawnChoserd];
                                Instantiate(Inimigo, new Vector3(spawnPosition.transform.position.x, spawnPosition.transform.position.y, spawnPosition.transform.position.z), Quaternion.identity);
                            }
                        }
                        //* ultimos 10% não da spawn
                        passedThrough = true;
                    break
                    ;
                    case 2:
                    //50% de chande de dar spawn ao inimigo
                        if(lucky>= 1 || lucky<=30)
                        {   
                            spawnChoserd = Random.Range(0, enemySpawnPoints.Length);
                            spawnPosition = enemySpawnPoints[spawnChoserd];
                            Instantiate(Inimigo, new Vector3(spawnPosition.transform.position.x, spawnPosition.transform.position.y, spawnPosition.transform.position.z), Quaternion.identity);
                        }
                        //*40% Chance de dar spawna a 2 inimigos
                        else if(lucky >=31 || lucky <= 70){
                            for(int i = 0; i < 2 ; i++) {
                                spawnChoserd = Random.Range(0, enemySpawnPoints.Length);
                                spawnPosition = enemySpawnPoints[spawnChoserd];
                                Instantiate(Inimigo, new Vector3(spawnPosition.transform.position.x, spawnPosition.transform.position.y, spawnPosition.transform.position.z), Quaternion.identity);
                            }
                        }
                         else if(lucky >=71 || lucky <= 80){
                            for(int i = 0; i < 3 ; i++) {
                                spawnChoserd = Random.Range(0, enemySpawnPoints.Length);
                                spawnPosition = enemySpawnPoints[spawnChoserd];
                                Instantiate(Inimigo, new Vector3(spawnPosition.transform.position.x, spawnPosition.transform.position.y, spawnPosition.transform.position.z), Quaternion.identity);
                            }
                        }

                    break;
                    case 3:
                        if(lucky>= 1 || lucky<=10)
                        {   
                            spawnChoserd = Random.Range(0, enemySpawnPoints.Length);
                            spawnPosition = enemySpawnPoints[spawnChoserd];
                            Instantiate(Inimigo, new Vector3(spawnPosition.transform.position.x, spawnPosition.transform.position.y, spawnPosition.transform.position.z), Quaternion.identity);
                        }
                        //*40% Chance de dar spawna a 2 inimigos
                        else if(lucky >=11 || lucky <= 40){
                            for(int i = 0; i < 2 ; i++) {
                                spawnChoserd = Random.Range(0, enemySpawnPoints.Length);
                                spawnPosition = enemySpawnPoints[spawnChoserd];
                                Instantiate(Inimigo, new Vector3(spawnPosition.transform.position.x, spawnPosition.transform.position.y, spawnPosition.transform.position.z), Quaternion.identity);
                            }
                        }
                         else if(lucky >=41 || lucky <= 80){
                            for(int i = 0; i < 4 ; i++) {
                                spawnChoserd = Random.Range(0, enemySpawnPoints.Length);
                                spawnPosition = enemySpawnPoints[spawnChoserd];
                                Instantiate(Inimigo, new Vector3(spawnPosition.transform.position.x, spawnPosition.transform.position.y, spawnPosition.transform.position.z), Quaternion.identity);
                            }
                        }
                    break;
                }
            }
        }
    }
}
