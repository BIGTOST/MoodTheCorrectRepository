using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToPreviousScene : MonoBehaviour
{
    private int previousSceneIndex; // Variável para armazenar o índice da cena anterior

    void Start()
    {
        // Inicialmente, definimos a cena anterior como a cena atual (para evitar índices inválidos)
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void GoBack()
    {
        // Carregar a cena anteriormente armazenada
        SceneManager.LoadScene(previousSceneIndex);
    }

    // Método para atualizar o índice da cena anterior ao mudar de cena
    public void UpdatePreviousSceneIndex()
    {
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
}
