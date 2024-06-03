using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject GameEnd;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject Options;


  

    void Awake()
    {
        gameOverScreen.SetActive(false);
        GameEnd.SetActive(false);
        PauseMenu.SetActive(false);
        Options.SetActive(false);
    }
    public void GameOver(){
        gameOverScreen.SetActive(true);
    }

    public void Pause(){
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
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
        Time.timeScale = 1;
    }
    public void WinGame(){
        GameEnd.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void MainMenu(){

        SceneManager.LoadScene("StartScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
