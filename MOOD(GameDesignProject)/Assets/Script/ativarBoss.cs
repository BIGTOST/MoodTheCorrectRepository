using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ativarBoss : MonoBehaviour
{
    public GameObject boss;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            Instantiate(boss, new Vector3(89.08f, 0.5751381f, -0.14f), Quaternion.identity);
        }
    }
}
