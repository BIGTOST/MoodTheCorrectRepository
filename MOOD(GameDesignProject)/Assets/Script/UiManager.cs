using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject GameEnd;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject Options;
    [SerializeField] private TimeText timeController;

    void Awake()
    {
        gameOverScreen.SetActive(false);
        GameEnd.SetActive(false);
        PauseMenu.SetActive(false);
        Options.SetActive(false);
        timeController = gameObject.GetComponent<TimeText>();
    }
    public void GameOver(){
        timeController.EndGame(false);
        gameOverScreen.SetActive(true);
    }

    public void Pause(){
        PauseMenu.SetActive(true);
        timeController.ZaWarudo();
    }

    public void OptionsOpen(){
        Options.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void OptionsClose(){
        Options.SetActive(false);
        PauseMenu.SetActive(true);
    }
    
    public void UnPause(){
        PauseMenu.SetActive(false);
        timeController.reverseZaWarudo();
    }

    public void WinGame(){
        timeController.EndGame(true);
        GameEnd.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level 1");
    }
    
    public void MainMenu(){

        SceneManager.LoadScene("StartScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
