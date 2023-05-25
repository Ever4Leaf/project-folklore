using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStateMachine : MonoBehaviour
{
    private BattleStateMachine battleStateMachine;
    public PlayerBase player;

    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    //for progress bar
    public float cur_cooldown;
    public float max_cooldown;
    public Image hpProgressBar;
    public Image apProgressBar;
    public GameObject selector;

    //IEnumerator
    public GameObject enemyTarget;
    public bool actionStarted = false;
    public Vector3 startingPosition;
    public float animSpeed = 5.0f;

    //Player Dead
    public bool alive = true;

    //Player Panel
    public PlayerPanelStatus playerStatus;
    public GameObject playerPanel;
    public Transform playerPanelSpacer;


    // Start is called before the first frame update
    void Start()
    {
        //find spacer
        playerPanelSpacer = GameObject.Find("Battle Canvas").transform.Find("Panel Character List").transform.Find("Panel Character List Spacer");
        //create panel, fill in info
        CreatePlayerStatusPanel();

        cur_cooldown = 0f;
        max_cooldown = 10f / player.speedStat;

        startingPosition = transform.position;
        selector.SetActive(false);
        currentState = TurnState.PROCESSING;
        battleStateMachine = GameObject.FindObjectOfType<BattleStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                    ProgressBar();
                break;

            case (TurnState.ADDTOLIST):
                    battleStateMachine.playerManageable.Add(this.gameObject);

                    currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                //idle state
                break;

            case (TurnState.SELECTING):

                break;

            case (TurnState.ACTION):
                    StartCoroutine(TimeForAction());

                break;

            case (TurnState.DEAD):
                if (!alive)
                {
                    return;
                }
                else
                {
                    //change tag
                    this.gameObject.tag = "DeadPlayer";
                    //not attackable by any enemy
                    battleStateMachine.playerInBattle.Remove(this.gameObject);
                    //not manageable
                    battleStateMachine.playerManageable.Remove(this.gameObject);
                    //deactivte selector
                    selector.SetActive(false);
                    //reset GUI
                    battleStateMachine.actionPanel.SetActive(false);
                    battleStateMachine.enemySelectPanel.SetActive(false);
                    //remove from performlist
                    for(int i = 0; i < battleStateMachine.performList.Count; i++)
                    {
                        if(battleStateMachine.performList[i].attackerGO == this.gameObject)
                        {
                            battleStateMachine.performList.Remove(battleStateMachine.performList[i]);
                        }
                    }
                    //change color/play animation
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
                    //reset playerinput
                    battleStateMachine.curr_battleState = BattleStateMachine.BattleStates.CHECKALIVE;

                    alive = false;
                }
                break;
        }
    }

    void ProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.ADDTOLIST;
        }
    }

    IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        //start coroutine
        actionStarted = true;

        //animate enemy to near the player
        Vector3 enemyTargetPosition = enemyTarget.transform.position + new Vector3(-0.2f, 0f, -1.25f);
        while (MoveTowardTarget(enemyTargetPosition)) { yield return null; }

        //wait
        yield return new WaitForSeconds(0.5f);
        //do damage
        DoDamage();
        //animate back to starting position
        Vector3 startPos = startingPosition;
        while (MoveTowardStart(startPos)) { yield return null; }

        //remove this(enemy action) from performList in BSM(battleStateMachine)
        battleStateMachine.performList.RemoveAt(0);
        //reset BSM then wait
        battleStateMachine.curr_battleState = BattleStateMachine.BattleStates.WAIT;

        //end coroutine
        actionStarted = false;
        //reset enemy state
        cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;
    }

    bool MoveTowardTarget(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    bool MoveTowardStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    //take damage
    public void TakeDamage(float getDamageAmount)
    {
        float calc_dmgTaken = (getDamageAmount - player.defendStat);
        player.currentHP -= calc_dmgTaken;
        if (player.currentHP <= 0f)
        {
            player.currentHP = 0f;
            currentState = TurnState.DEAD;
        }
        UpdatePlayerPanel();
    }

    //do damage
    public void DoDamage()
    {
        float calc_playerDmg = player.attackStat + battleStateMachine.performList[0].usedAttack.attackDamage;
        enemyTarget.GetComponent<EnemyStateMachine>().TakeDamage(calc_playerDmg);
    }

    void CreatePlayerStatusPanel()
    {
        playerPanel = Instantiate(playerPanel) as GameObject;
        playerStatus = playerPanel.GetComponent<PlayerPanelStatus>();

        playerStatus.playerName.text = player.unitName;
        playerStatus.playerHP.text = player.currentHP + "/" + player.maxHP; // Ex. 40/100
        
        //add progress bar

        playerPanel.transform.SetParent(playerPanelSpacer, false);
    }

    void UpdatePlayerPanel()
    {
        playerStatus.playerHP.text = player.currentHP + "/" + player.maxHP; // Ex. 40/100
    }
}
