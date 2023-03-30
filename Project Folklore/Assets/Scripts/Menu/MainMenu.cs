using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public string newGameScene;

    public AudioMixer audioMixer;


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

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("bgm", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfx", volume);
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
