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
    [Header("Player Status")]
    public TurnState currentState;

    //for progress bar
    public float cur_cooldown;
    public float max_cooldown;
    private Image hpProgressBar;
    private Image apProgressBar;
    public GameObject selector;

    //Player Dead
    public bool alive = true;

    [Header("For IEnumerator Purpose")]
    //IEnumerator
    public GameObject enemyTarget;
    public bool actionStarted = false;
    public Vector3 startingPosition;
    public float animSpeed = 5.0f;

    [Header("Player Panel")]
    //Player Panel
    public PlayerPanelStatus playerStatus;
    public GameObject playerPanel;
    public Transform playerPanelSpacer;

    // Start is called before the first frame update
    void Start()
    {
        //find spacer
        playerPanelSpacer = GameObject.Find("GUIs").transform.Find("Canvas Character").transform.Find("Panel Character List").transform.Find("Panel Character List Spacer");
        //playerPanelSpacer = GameObject.Find("Battle Canvas").transform.Find("Panel Character List").transform.Find("Panel Character List Spacer");
        //create panel, fill in info
        CreatePlayerStatusPanel();

        //set animation to idle
        player.playerAnimator.SetTrigger("Idle");

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
                    //set animation to idle
                    player.playerAnimator.SetTrigger("Idle");

                    ProgressBar();
                break;

            case (TurnState.ADDTOLIST):
                    battleStateMachine.playerManageable.Add(this.gameObject);

                    currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                    //idle state
                    //set animation to idle
                    player.playerAnimator.SetTrigger("Idle");
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
                    if (battleStateMachine.playerInBattle.Count > 0)
                    {
                        for (int i = 0; i < battleStateMachine.performList.Count; i++)
                        {
                            if (i != 0)
                            {
                                if (battleStateMachine.performList[i].attackerGO == this.gameObject)
                                {
                                    battleStateMachine.performList.Remove(battleStateMachine.performList[i]);
                                }

                                if (battleStateMachine.performList[i].attackTarget == this.gameObject)
                                {
                                    battleStateMachine.performList[i].attackTarget = battleStateMachine.playerInBattle[Random.Range(0, battleStateMachine.playerInBattle.Count)];
                                }
                            }
                        }
                    }

                    //change color or play animation death
                    player.playerAnimator.SetTrigger("Dead");
                    player.playerAnimator.ResetTrigger("Idle");
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);

                    //call checkalive state
                    battleStateMachine.curr_battleState = BattleStateMachine.BattleStates.CHECKALIVE;

                    //set alive=false
                    alive = false;
                }
                break;
        }
    }

    void ProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;

        apProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), apProgressBar.transform.localScale.y, apProgressBar.transform.localScale.z);
        
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
        Vector3 enemyTargetPosition = enemyTarget.transform.position + new Vector3(-1.25f, 0f, 0f);
        while (MoveTowardTarget(enemyTargetPosition)) { yield return null; }

        //do damage
        DoDamage();

        //wait
        yield return new WaitForSeconds(1.5f);
        
        //animate back to starting position
        Vector3 startPos = startingPosition;
        while (MoveTowardStart(startPos)) { yield return null; }

        //remove this(player) from performList in BSM(battleStateMachine)
        battleStateMachine.performList.RemoveAt(0);

        //reset BSM then wait
        if(battleStateMachine.curr_battleState != BattleStateMachine.BattleStates.WIN && battleStateMachine.curr_battleState != BattleStateMachine.BattleStates.LOSE)
        {
            battleStateMachine.curr_battleState = BattleStateMachine.BattleStates.WAIT;

            //reset player state
            cur_cooldown = 0f;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }

        //end coroutine
        actionStarted = false;
    }

    bool MoveTowardTarget(Vector3 target)
    {
        //set animation to walking
        //playerAnimate.SetTrigger("Walking");
        player.playerAnimator.ResetTrigger("Idle");

        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    bool MoveTowardStart(Vector3 target)
    {
        //set animation to walking
        //playerAnimate.SetTrigger("Walking");
        player.playerAnimator.ResetTrigger("Idle");

        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    //take damage
    public void TakeDamage(float getDamageAmount)
    {
        //set animation to take damage

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
        //set animation to attack
        player.playerAnimator.SetTrigger("Attack");
        player.playerAnimator.ResetTrigger("Idle");

        float calc_playerDmg = player.attackStat + battleStateMachine.performList[0].usedAttack.attackDamage;
        enemyTarget.GetComponent<EnemyStateMachine>().TakeDamage(calc_playerDmg);
    }

    public void PlayerStatusBar()
    {
        float calc_hp = player.currentHP / player.maxHP; //clamp value to 1
        float calc_ap = player.currentAP / player.maxAP; 

        hpProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_hp, 0, 1), hpProgressBar.transform.localScale.y, hpProgressBar.transform.localScale.z);
        apProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_ap, 0, 1), apProgressBar.transform.localScale.y, apProgressBar.transform.localScale.z);
    }

    void CreatePlayerStatusPanel()
    {
        playerPanel = Instantiate(playerPanel) as GameObject;
        playerStatus = playerPanel.GetComponent<PlayerPanelStatus>();

        playerStatus.playerName.text = player.unitName;
        playerStatus.playerHP.text = player.currentHP + "/" + player.maxHP; // Ex. 40/100

        hpProgressBar = playerStatus.hpBar;
        apProgressBar = playerStatus.apBar;

        PlayerStatusBar();
        playerPanel.transform.SetParent(playerPanelSpacer, false);
    }

    void UpdatePlayerPanel()
    {
        playerStatus.playerHP.text = player.currentHP + "/" + player.maxHP; // Ex. 40/100
        PlayerStatusBar();
    }
}
