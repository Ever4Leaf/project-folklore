using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerPrefab;

    public Vector3 nextPlayerPosition;
    public Vector3 lastPlayerPosition;

    public string sceneToLoad;
    public string lastScene;

    private void Awake()
    {
        //check if instance exist
        if (instance == null)
        {
            //if not then set instance to this
            instance = this;
        }
        //if there is but not this instance
        else if (instance != this)
        {
            //then destroy it
            Destroy(gameObject);
        }
        //set this instance to be not destroyable
        DontDestroyOnLoad(gameObject);

        if (!GameObject.Find("Ken Arok"))
        {
            GameObject Player = Instantiate(playerPrefab, nextPlayerPosition, Quaternion.identity) as GameObject;
            Player.name = "Player";
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
