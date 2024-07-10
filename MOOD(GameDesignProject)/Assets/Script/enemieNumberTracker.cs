using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemieNumberTracker : MonoBehaviour
{
    private int numberOfEnemies;
    public int getNumberOfEnemies()
    {
        return this.numberOfEnemies;
    }
    public void setNumberOfEnemies(int add)
    {
        numberOfEnemies+=add;
    }

}
