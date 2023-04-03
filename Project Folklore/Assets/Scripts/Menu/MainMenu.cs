using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string newGameScene;

    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    
    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void Settings()
    {
        mainMenuPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        mainMenuPanel.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);
    }
}
