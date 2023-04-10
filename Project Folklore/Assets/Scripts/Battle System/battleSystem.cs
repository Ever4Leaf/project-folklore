using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, CHANGETURNTRANSITION, CHOOSETARGET }

public class battleSystem : MonoBehaviour
{
    //Prefab for player && enemy
    [Header("Prefab Entity")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    //Getting 3D position of player && enemy battle station (for spawning prefab)
    [Header("Battle Station Location")]
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    //Battle HUD for player && enemy
    [Header("Battle HUD")]
    public battleHUD playerHUD;
    public battleHUD enemyHUD;

    //States of battle
    [Header("State of the Battle")]
    public BattleState state;
    public TextMeshProUGUI battleDialogueText;
    public TextMeshProUGUI battleStateText;

    //Other Settings
    [Header("Other Settings")]
    public float waitTime = 2f;

    //containing the unitModifier class for player && enemy unit
    unitModifier playerUnit;
    unitModifier enemyUnit;

    //Battle Setup
    void Start()
    {
        state = BattleState.START;
        Debug.Log(state);
        setupBattle();
    }

    private void setupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<unitModifier>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<unitModifier>();

        playerHUD.setHUD(playerUnit);
        enemyHUD.setHUD(enemyUnit);

        battleDialogueText.text = "A Wild " + enemyUnit.unitName + " Is Approaching...";

        state = BattleState.PLAYERTURN;
        Debug.Log(state);
        StartCoroutine(changeTurnTransition());
    }

    //TURN CHANGE TRANSITION
    private IEnumerator changeTurnTransition()
    {
        battleStateText.text = "STAND BY";
        yield return new WaitForSeconds(2f);   

        if (state == BattleState.PLAYERTURN)
        {
            playerTurn();
        }
        else if (state == BattleState.ENEMYTURN)
        {
            StartCoroutine(enemyTurn());
        }
    }

    //PLAYERTURN && ENEMYTURN
    private IEnumerator enemyTurn()
    {
        //SET DISPLAY TO "ENEMY TURN"
        battleStateText.text = "ENEMY TURN";
        yield return new WaitForSeconds(waitTime);

        //CHECK FOR STATUS EFFECT
        //DAMAGE CALCULATION
        //CHECK ENEMY HP

        //ENEMY ACTION
        battleDialogueText.text = enemyUnit.unitName + " used Attack";
        yield return new WaitForSeconds(waitTime);

        //DAMAGE PLAYER && UPDATE HUD
        bool isDead = playerUnit.takeDamage(enemyUnit.attackStat);
        playerHUD.setHP(playerUnit.currentHP);

        battleDialogueText.text = playerUnit.unitName + " take " + enemyUnit.attackStat + " damage";
        yield return new WaitForSeconds(waitTime);

        //CHECK PLAYER HP AFTER ACTION
        //CHANGE STATE BASED ON CONDITION
        if (isDead == true)
        {
            //END BATTLE
            state = BattleState.LOST;
            Debug.Log(state);
            endBattle();
        }
        else if (isDead == false)
        {
            //CHANGE TO PLAYER TURN
            state = BattleState.PLAYERTURN;
            Debug.Log(state);
            StartCoroutine(changeTurnTransition());
        }
    }

    private void playerTurn()
    {
        //SET DISPLAY TO "PLAYERTURN"
        battleStateText.text = "PLAYER TURN";
        battleDialogueText.text = "What will " + playerUnit.unitName + " do...";

        //CHECK FOR STATUS EFFECT
        //CALCULATE DAMAGE

        //CHECK PLAYER HP
        //CHANGE STATE BASED ON CONDITION
    }

    //PLAYER ACTION
    public void onAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(playerAttack());
            battleDialogueText.text = playerUnit.unitName + " used Attack";
        }
    }

    public void onHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(playerAttack());
            battleDialogueText.text = playerUnit.unitName + " used Heal";
        }
    }

    private IEnumerator playerAttack()
    {
        //IMMEDIATELY CHANGE STATE
        state = BattleState.CHANGETURNTRANSITION;
        Debug.Log(state);
        yield return new WaitForSeconds(waitTime);

        //DAMAGE THE ENEMY
        bool isDead = enemyUnit.takeDamage(playerUnit.attackStat);
        enemyHUD.setHP(enemyUnit.currentHP);

        battleDialogueText.text = enemyUnit.unitName + " take " + playerUnit.attackStat + " damage";
        yield return new WaitForSeconds(waitTime);

        //CHECK ENEMY HP
        //CHANGE STATE BASED ON CONDITION
        if (isDead == true)
        {
            //END BATTLE
            state = BattleState.WON;
            Debug.Log(state);
            endBattle();
        }
        else if (isDead == false)
        {
            //CHANGE TO ENEMY TURN
            state = BattleState.ENEMYTURN;
            Debug.Log(state);
            StartCoroutine(changeTurnTransition());
        }
    }

    private IEnumerator playerHeal()
    {
        //IMMEDIATELY CHANGE STATE
        state = BattleState.CHANGETURNTRANSITION;
        Debug.Log(state);
        yield return new WaitForSeconds(waitTime);

        //HEAL PLAYER FOR...
        playerUnit.recoverHP(playerUnit.recoveryStat);
        battleDialogueText.text = playerUnit.attackStat + " recover " + playerUnit.recoveryStat + " HP";
        playerHUD.setHP(playerUnit.currentHP);
        yield return new WaitForSeconds(waitTime);

        //CHANGE TO ENEMY TURN
        state = BattleState.ENEMYTURN;
        Debug.Log(state);
        StartCoroutine(changeTurnTransition());
    }

    //END BATTLE
    public void endBattle()
    {
        if (state == BattleState.WON)
        {
            battleDialogueText.text = playerUnit.unitName + " Won the battle";
            battleStateText.text = "BATTLE END";
        }
        else if (state == BattleState.LOST)
        {
            battleDialogueText.text = playerUnit.unitName + " Lost the battle";
            battleStateText.text = "BATTLE END";
        }
    }
}
