using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 --> Sala com Porta em baixo
    // 2 --> Sala com Porta em Cima
    // 3 --> Sala com Porta em Esquerda
    // 4 --> Sala com Porta em Direita
    
    private RoomTemplates templates;
    private int randomValue;
    private bool IsRoomSpawned;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        //Chama a função spawn com um pequeno tempo entre calls
        Debug.Log($"MaxRooms{templates.maxOfRooms}, RoomsCreated:{templates.roomsCreated}");
        if(templates.maxOfRooms>= templates.roomsCreated)
        {
            Invoke("Spawn", 0.1f);
        }
        
    }

    private void Spawn() {
        //para garantir que apenas uma sala apareça na posição de spawn
        if(!IsRoomSpawned){
            switch(openingDirection){
                case 1:
                    //Irá selecionar uma das salas do vetor de salas com portas para baixo e gerar na opição
                    randomValue = Random.Range(0, templates.BottomRooms.Length);
                    //Cria a sala na posição selecionada no randomValue
                    Instantiate(templates.BottomRooms[randomValue], transform.position, templates.BottomRooms[randomValue].transform.rotation);
                    //templates.roomsCreated++;
                    break;
                case 2:
                    randomValue = Random.Range(0, templates.TopRooms.Length);
                    Instantiate(templates.TopRooms[randomValue], transform.position, templates.TopRooms[randomValue].transform.rotation);
                    //templates.roomsCreated++;
                    break;
                case 3:
                    randomValue = Random.Range(0, templates.RightRooms.Length);
                    Instantiate(templates.RightRooms[randomValue], transform.position, templates.RightRooms[randomValue].transform.rotation);
                    //templates.roomsCreated++;
                    break;
                case 4:
                    randomValue = Random.Range(0, templates.LeftRooms.Length);
                    Instantiate(templates.LeftRooms[randomValue], transform.position, templates.LeftRooms[randomValue].transform.rotation);
                    //templates.roomsCreated++;
                    break;
                
            }
            IsRoomSpawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("SpawnPoint"))
        {
            if(other.GetComponent<RoomSpawner>().IsRoomSpawned == false && IsRoomSpawned == false){
                Instantiate(templates.closeRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
