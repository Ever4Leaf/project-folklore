using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOSE}

public class battleSystem : MonoBehaviour
{
    public GameObject palyerPrefab;
    public GameObject enemyPrefab;

    public Transform enemyBattleStation;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        setupBattle();
    }

    private void setupBattle()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
