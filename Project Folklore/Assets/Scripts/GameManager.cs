using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    [Header("States")]
    public bool isWalking = false;
    public bool canGetEncounter = false;
    public bool gotAttacked = false;

    //ENUM
    public enum GammeStates 
    {
        WORLD_STATE,
        VILLAGE_STATE,
        BATTLE_STATE,
        MENU_STATE,
        IDLE_STATE
    }
    [Header("Battle Setup")]
    public int enemyAmount;
    public GammeStates curr_GameState;
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

        if (!GameObject.Find("Ken Arok"))
        {
            GameObject Player = Instantiate(playerPrefab, nextPlayerPosition, Quaternion.identity) as GameObject;
            Player.name = "Player";
        }
    }

    void Update()
    {
        switch (curr_GameState)
        {
            case (GammeStates.WORLD_STATE):
                if (isWalking)
                {
                    RandomEncounter();
                }

                if (gotAttacked)
                {
                    curr_GameState = GammeStates.BATTLE_STATE;
                }
                break;

            case (GammeStates.VILLAGE_STATE):

                break;

            case (GammeStates.BATTLE_STATE):
                //Load Battle Scene
                StartBattle();
                //Set to Idle
                curr_GameState = GammeStates.IDLE_STATE;
                break;

            case (GammeStates.MENU_STATE):

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

    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene);
    }

    public void RandomEncounter()
    {
        if (isWalking && canGetEncounter)
        {
            if (Random.Range(0,1000) < 10)
            {
                Debug.Log("Boom");
                gotAttacked = true;
            }
        }
    }

    public void StartBattle()
    {
        //Amount of Enemy
        enemyAmount = Random.Range(1, curr_region.maxAmountEnemy + 1);
        //Which Enemy
        for (int i = 0; i < enemyAmount; i++)
        {
            enemyToBattle.Add(curr_region.possibleEnemy[Random.Range(0, curr_region.possibleEnemy.Count)]);
        }
        //Player
        lastPlayerPosition = GameObject.Find("Player").gameObject.transform.position;
        nextPlayerPosition = lastPlayerPosition;
        lastScene = SceneManager.GetActiveScene().name;
        //Load Level
        SceneManager.LoadScene(curr_region.battleScene);
        //Reset Hero
        isWalking = false;
        canGetEncounter = false;
        gotAttacked = false;
    }
}
