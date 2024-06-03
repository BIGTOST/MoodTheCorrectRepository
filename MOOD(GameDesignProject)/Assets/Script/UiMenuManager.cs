using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject Options;
    [SerializeField] private GameObject Creditos;

    void Awake()
    {
        Options.SetActive(false);
        Creditos.SetActive(false);
    }


    public void OptionsOpen(){
        Options.SetActive(true);
    }

    public void OptionsClose(){
        Options.SetActive(false);
    }
    

    public void OpenCredits()
    {
        Creditos.SetActive(true);
    }
    public void CloseCredits()
    {
        Creditos.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    

    public void Quit()
    {
        Application.Quit();
    }

}
