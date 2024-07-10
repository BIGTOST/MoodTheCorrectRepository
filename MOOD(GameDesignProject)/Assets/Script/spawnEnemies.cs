using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemies : MonoBehaviour
{
    private bool passedThrough;
    public enemieNumberTracker tracker;
    [SerializeField]private GameObject[] Inimigos;

    private int level=1;

    void Awake()
    {
        passedThrough = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            if(!passedThrough)
            {
                switch(level){

                }
            }
        }
    }
}
