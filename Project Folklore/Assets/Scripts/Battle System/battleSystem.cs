using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOSE}

public class battleSystem : MonoBehaviour
{
    //Prefab for player && enemy
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    //Getting 3D position of player && enemy battle station (for spawning prefab)
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    //containing the unitModifier class for player && enemy unit
    unitModifier playerUnit;
    unitModifier enemyUnit;

    //Battle HUD for player && enemy
    public battleHUD playerHUD;
    public battleHUD enemyHUD;

    //States of battle
    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
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
    }

}
