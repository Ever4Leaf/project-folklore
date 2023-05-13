using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerBase 
{
    [Header("Player Informations")]
    //Generic information
    public string unitName;
    public int unitLevel;

    //Player Health Point && Action Point
    public int maxHP;
    public int currentHP;
    public int maxAP;
    public int currentAP;

    //Player Stats Modifier
    [Header("Player Stats")]
    public int attackStat;
    public int defendStat;
    public int speedStat;
    public int recoveryStat;
    public int costAP;
}
