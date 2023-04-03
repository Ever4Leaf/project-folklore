using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool SettingIsOpen = false;

    public string menuScene;

    public GameObject pausePanel;

    public GameObject settingsPanel;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !SettingIsOpen) 
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Settings()
    {
        pausePanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(true);
        SettingIsOpen = true;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    public void BackToPause()
    {
        pausePanel.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);
        SettingIsOpen = false;
    }
}
