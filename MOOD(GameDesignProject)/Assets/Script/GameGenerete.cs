using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGenerete : MonoBehaviour
{
    public Material[] materialsList;
    public GameObject[] todasAsSalas;
    public int mapaHeight,//numero de linhas
            mapLenght;//numero de colunas

    private float[,] mapMatix;
    private float posX = 0;
    private float aux = 0;
    private float posZ = 0;


    private void Awake()
    {
        mapLenght = Random.Range(3,10);
        mapaHeight = Random.Range(3,10);
        mapMatix = new float[mapLenght,mapaHeight];
        
        for(int l =0;l < mapLenght ; l++){
            for(int c = 0; c < mapaHeight; c++){
                int randomIndex = Random.Range(0, materialsList.Length+1);
                mapMatix[l,c]=randomIndex;
                CreateRoom(randomIndex, l, c);
                //Debug.Log($"Position Aux{aux}");
            }
            posZ = 0;
            posX += aux;
            Debug.Log($"{posX}");
        }
        Debug.Log(
            "["+$"{mapMatix[0,0]},{mapMatix[0,1]},{mapMatix[0,2]}"+"]\n"+
            "["+$"{mapMatix[1,0]},{mapMatix[1,1]},{mapMatix[1,2]}"+"]\n"+
            "["+$"{mapMatix[2,0]},{mapMatix[2,1]},{mapMatix[2,2]}"+"]\n");
    }

    private void CreateRoom(int roomNumber, int XdaSala, int YdaSala){
        int qualSala = Random.Range(0,todasAsSalas.Length);
        //?cria o plano
        GameObject sala = Instantiate(todasAsSalas[qualSala], new Vector3(posX, 1, posZ), Quaternion.identity);
        // GameObject.CreatePrimitive(PrimitiveType.Plane);
        //?Posiciona o plano
        //sala.transform.position = new Vector3(posX, 1, posZ);
        MeshRenderer meshRenderer = sala.GetComponentInChildren<MeshRenderer>();

        Vector3 size = meshRenderer.bounds.size;
        posZ -= size.z;
        //Debug.Log($"PositionZ:{size.z}");
        aux = size.x;
       //Debug.Log($"PositionXAux:{size.x}");
    //     Debug.Log($"x:{XdaSala} Y:{YdaSala}");
    //     if(roomNumber !=0){
    //         if(XdaSala == 0 || XdaSala == (mapLenght-1) || YdaSala == 0 || YdaSala == (mapaHeight-1)){

    //             meshRenderer.material = materialsList[0];
    //         }else{
    //             
    //             #region mudarCor
    //             meshRenderer.material = materialsList[qualSala];
    //             #endregion
    //         }
    //     }
    }
}
