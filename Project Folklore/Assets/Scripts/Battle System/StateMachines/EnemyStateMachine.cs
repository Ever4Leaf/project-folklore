using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private BattleStateMachine battleStateMachine;
    public EnemyBase enemy;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }
    [Header("Enemy Status")]
    public TurnState currentState;

    //for progress bar
    public float cur_cooldown;
    public float max_cooldown;
    public GameObject enemySelector;

    //Enemy check alive
    public bool enemyAlive = true;

    [Header("For IEnumerator Purpose")]
    //IEnumerator
    public bool actionStarted = false;
    public GameObject playerTarget;
    public float animSpeed = 5.0f;
    public Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        cur_cooldown = 0f;
        max_cooldown = max_cooldown / enemy.speedStat;

        currentState = TurnState.PROCESSING;
        enemySelector.SetActive(false);
        battleStateMachine = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                ProgressBar();
                break;

            case (TurnState.CHOOSEACTION):
                ChooseAction();

                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                //idle state
                break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());

                break;

            case (TurnState.DEAD):
                if (!enemyAlive)
                {
                    return;
                }
                else
                {
                    //change enemy tag
                    this.gameObject.tag = "DeadEnemy";
                    //remove enemy game object
                    battleStateMachine.enemyInBattle.Remove(this.gameObject);
                    //deactivte selector
                    enemySelector.SetActive(false);

                    //remove inputs of dead enemy from performlist
                    if (battleStateMachine.enemyInBattle.Count > 0)
                    {
                        for (int i = 0; i < battleStateMachine.performList.Count; i++)
                        {
                            if(i != 0)
                            {
                                if (battleStateMachine.performList[i].attackerGO == this.gameObject)
                                {
                                    battleStateMachine.performList.Remove(battleStateMachine.performList[i]);
                                }

                                if (battleStateMachine.performList[i].attackTarget = this.gameObject)
                                {
                                    battleStateMachine.performList[i].attackTarget = battleStateMachine.enemyInBattle[Random.Range(0, battleStateMachine.enemyInBattle.Count)];
                                }
                            }
                        }
                    }

                    //change color or play animation
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);

                    //set enemyAlive=false
                    enemyAlive = false;

                    //reset enemy button
                    battleStateMachine.CreateEnemyButtons();

                    //call checkalive state
                    battleStateMachine.curr_battleState = BattleStateMachine.BattleStates.CHECKALIVE;
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
            currentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction()
    {
        TurnHandler enemyAction = new TurnHandler();
        enemyAction.attackerName = enemy.unitName;
        enemyAction.attackerType = "Enemy";
        enemyAction.attackerGO = this.gameObject;
        if (battleStateMachine.playerInBattle.Count > 0)
        {
            enemyAction.attackTarget = battleStateMachine.playerInBattle[Random.Range(0, battleStateMachine.playerInBattle.Count)];
        }

        int num = Random.Range(0, enemy.attackList.Count);
        enemyAction.usedAttack = enemy.attackList[num];

        battleStateMachine.GetActionInfoFrom(enemyAction);
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
        Vector3 playerTargetPosition = playerTarget.transform.position + new Vector3(1.25f, 0f, 0f);
        while (MoveTowardTarget(playerTargetPosition)) { yield return null; }

        //wait
        yield return new WaitForSeconds(0.5f);
        
        //do damage
        DoDamage();
        
        //animate back to starting position
        Vector3 startPos = startingPosition;
        while (MoveTowardStart(startPos)) { yield return null; }

        //remove this(enemy) from performList in BSM(battleStateMachine)
        battleStateMachine.performList.RemoveAt(0);

        //reset BSM then wait
        if (battleStateMachine.curr_battleState != BattleStateMachine.BattleStates.WIN && battleStateMachine.curr_battleState != BattleStateMachine.BattleStates.LOSE)
        {
            battleStateMachine.curr_battleState = BattleStateMachine.BattleStates.WAIT;

            //reset enemy state
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
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    bool MoveTowardStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    //do damage
    public void DoDamage()
    {
        float calc_enemyDmg = enemy.attackStat + battleStateMachine.performList[0].usedAttack.attackDamage;

        playerTarget.GetComponent<PlayerStateMachine>().TakeDamage(calc_enemyDmg);
    }

    //take damage
    public void TakeDamage(float getDamageAmount)
    {
        enemy.currentHP -= getDamageAmount;
        if(enemy.currentHP <= 0)
        {
            enemy.currentHP = 0;
            currentState = TurnState.DEAD;
        }
    }
}
