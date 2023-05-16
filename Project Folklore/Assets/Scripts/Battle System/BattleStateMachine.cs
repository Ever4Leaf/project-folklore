using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleStateMachine : MonoBehaviour
{
    public enum BattleStates
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }
    public BattleStates curr_battleState;

    public List<TurnHandler> performList = new List<TurnHandler>();
    public List<GameObject> playerInBattle = new List<GameObject>();
    public List<GameObject> enemiesInbattle = new List<GameObject>();

    public enum PlayerGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }
    public PlayerGUI playerInput;

    public List<GameObject> playerManager = new List<GameObject>();
    private TurnHandler playerSelect;

    public GameObject enemyButton;
    public Transform spacer;

    public GameObject actionPanel;
    public GameObject enemySelectPanel;

    // Start is called before the first frame update
    void Start()
    {
        curr_battleState = BattleStates.WAIT;
        GetPlayerGO();
        GetEnemyGO();

        playerInput = PlayerGUI.ACTIVATE;

        EnemyButtons();
    }

    // Update is called once per frame
    void Update()
    {
        switch (curr_battleState)
        {
            case (BattleStates.WAIT):
                    if (performList.Count > 0)
                    {
                        curr_battleState = BattleStates.TAKEACTION;
                    }
                break;

            case (BattleStates.TAKEACTION):
                    GameObject actionPerformer = GameObject.Find(performList[0].attackerName);
                    
                    if(performList[0].attackerType == "Player")
                    {
                        PlayerStateMachine playerStateMachine = actionPerformer.GetComponent<PlayerStateMachine>();
                    }

                    if (performList[0].attackerType == "Enemy")
                    {
                        EnemyStateMachine enemyStateMachine = actionPerformer.GetComponent<EnemyStateMachine>();
                        enemyStateMachine.targetAttack = performList[0].attackTarget;
                        enemyStateMachine.currentState = EnemyStateMachine.TurnState.ACTION;
                    }

                    curr_battleState = BattleStates.PERFORMACTION;
                break;

            case (BattleStates.PERFORMACTION):

                break;
        }

        switch (playerInput)
        {
            case (PlayerGUI.ACTIVATE):

                break;

            case (PlayerGUI.WAITING):

                break;

            case (PlayerGUI.INPUT1):

                break;

            case (PlayerGUI.INPUT2):

                break;

            case (PlayerGUI.DONE):

                break;
        }
    }

    public void GetActionInfoFrom(TurnHandler actionInfo)
    {
        performList.Add(actionInfo);
    }

    public void EnemyButtons()
    {
        foreach (GameObject enemy in enemiesInbattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton eButton = enemyButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine curr_enemy = enemy.GetComponent<EnemyStateMachine>();

            TextMeshProUGUI buttonText = newButton.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
            buttonText.text = curr_enemy.enemy.unitName;

            eButton.enemyPrefab = enemy;

            newButton.transform.SetParent(spacer, false);
        }
    }

    public void GetPlayerGO()
    {
        GameObject[] playerGO = GameObject.FindGameObjectsWithTag("Player1");
        foreach (GameObject pGO in playerGO)
        {
            playerInBattle.Add(pGO);
        }
    }

    public void GetEnemyGO()
    {
        GameObject[] enemyGO = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject eGO in enemyGO)
        {
            enemiesInbattle.Add(eGO);
        }
    }
}
