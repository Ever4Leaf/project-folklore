using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public BattleStateMachine battleStateMachine;
    public EnemyBase enemy;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }
    public TurnState currentState;

    public Vector3 startingPosition;

    private bool actionStarted = false;
    public GameObject targetAttack;
    public float animSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        currentState = TurnState.PROCESSING;
        battleStateMachine = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                currentState = TurnState.CHOOSEACTION;

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

                break;
        }
    }

    void ChooseAction()
    {
        TurnHandler enemyAction = new TurnHandler();
        enemyAction.attackerName = enemy.unitName;
        enemyAction.attackerType = "Enemy";
        enemyAction.attackerGO = this.gameObject;
        enemyAction.attackTarget = battleStateMachine.playerInBattle[Random.Range(0, battleStateMachine.playerInBattle.Count)];
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
        Vector3 targetPosition = targetAttack.transform.position + new Vector3(0.2f, 0f, 1.25f);
        while (MoveTowardTarget(targetPosition)) { yield return null; }

        //wait
        yield return new WaitForSeconds(0.5f);
        //do damage

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
}
