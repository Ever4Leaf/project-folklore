using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private UI_Manager UI_Manager;

    public RegionData curr_region;
    public string nextSpawnPoint;

    [Header("Player Prefab")]
    public GameObject playerPrefab;

    [Header("Player Position")]
    public Vector3 nextPlayerPosition;
    public Vector3 lastPlayerPosition;

    [Header("Scenes")]
    public string sceneToLoad;
    public string lastScene;
    public string newScene;

    [Header("States")]
    public bool isWalking = false;
    public bool canGetEncounter = false;
    public bool gotAttacked = false;
    public bool gameIsPaused = false;
    public bool isStaticEncounter = false;

    //ENUM
    public enum GammeStates 
    {
        WORLD_STATE,
        BATTLE_STATE,
        MENU_STATE,
        IDLE_STATE
    }
    [Header("Battle Setup")]
    public int enemyAmount;
    public GammeStates curr_GameState;
    public List<GameObject> playerParty = new List<GameObject>();
    public List<GameObject> enemyToBattle = new List<GameObject>();

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

        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            GameObject Player = Instantiate(playerPrefab, nextPlayerPosition, Quaternion.identity) as GameObject;
            Player.name = playerPrefab.name;
        }
    }

    void Update()
    {
        switch (curr_GameState)
        {
            case (GammeStates.WORLD_STATE):
                Time.timeScale = 1f;
                gameIsPaused = false;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                if (isWalking)
                {
                    RandomEncounter();
                }

                if (gotAttacked)
                {
                    curr_GameState = GammeStates.BATTLE_STATE;
                }

                if (isStaticEncounter)
                {
                    StaticEncounter();
                }
                break;

            case (GammeStates.BATTLE_STATE):
                //Load Battle Scene
                StartBattle();
                //Set to Idle
                curr_GameState = GammeStates.IDLE_STATE;
                break;

            case (GammeStates.MENU_STATE):
                Time.timeScale = 0f;
                gameIsPaused = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    UI_Manager.panelInventory.SetActive(false);

                    curr_GameState = GammeStates.WORLD_STATE;
                }

                break;

            case (GammeStates.IDLE_STATE):
                //IDLE STATE
                break;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void SearchNewNextScene()
    {
        newScene = NewSceneSelector.instance.newSceneSelector;
    }

    public void LoadSceneAfterBattle()
    {
        SearchNewNextScene();

        if (newScene != "")
        {
            //nextPlayerPosition = new Vector3(0f, 0f, 0f);
            SceneManager.LoadScene(newScene);
            newScene = "";
        }
        else
        {
            SceneManager.LoadScene(lastScene);
        }
    }

    public void RandomEncounter()
    {
        if (isWalking && canGetEncounter)
        {
            if (Random.Range(0,515)+1 <= 10)
            {
                Debug.Log("Random Battle");
                gotAttacked = true;
            }
        }
    }

    public void StaticEncounter()
    {
        if (isStaticEncounter)
        {
                Debug.Log("Static Battle");
                gotAttacked = true;
        }
    }

    public void StartBattle()
    {
        //Amount of Enemy
        enemyAmount = curr_region.maxAmountEnemy;
        //Which Enemy
        for (int i = 0; i < enemyAmount; i++)
        {
            enemyToBattle.Add(curr_region.possibleEnemy[Random.Range(0, curr_region.possibleEnemy.Count)]);
        }
        //Player
        lastPlayerPosition = GameObject.FindGameObjectWithTag("Player").gameObject.transform.position;
        nextPlayerPosition = lastPlayerPosition;
        lastScene = SceneManager.GetActiveScene().name;
        //Load Level
        SceneManager.LoadScene(curr_region.battleScene);
        //Reset Hero
        isWalking = false;
        canGetEncounter = false;
        gotAttacked = false;
        isStaticEncounter = false;
    }
}
