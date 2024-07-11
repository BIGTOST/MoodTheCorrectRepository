using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Options;
    [SerializeField] private GameObject Creditos;

    void Awake()
    {
        Options.SetActive(false);
        Creditos.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void OptionsOpen(){
        Options.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void OptionsClose()
    {
        Options.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void OpenCredits()
    {
        Creditos.SetActive(true);
        MainMenu.SetActive(false);
    }
    public void CloseCredits()
    {
        Creditos.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
