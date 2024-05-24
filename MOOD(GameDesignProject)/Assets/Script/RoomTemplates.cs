using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    // Arrays com a lista das salas que serão usadas no jogo
    public GameObject[] AllRooms,
                        TopRooms,
                        RightRooms,
                        LeftRooms,
                        BottomRooms;

    public GameObject closeRoom;
    public GameObject Destroyer;
    public int maxOfRooms , roomsCreated;
    // Start is called before the first frame update
    private int randomValue;
    private bool IsFirstRoomsSpawned;
    void Start()
    {
        if(!IsFirstRoomsSpawned){
            GameObject  FirstRoom =Instantiate(AllRooms[randomValue], new Vector3(0, 0, 0) , AllRooms[randomValue].transform.rotation);
            randomValue = Random.Range(0, AllRooms.Length);
            Instantiate(Destroyer, new Vector3(0,0,0), Destroyer.transform.rotation, FirstRoom.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
