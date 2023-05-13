using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyBase 
{
    [Header("Enemy Informations")]
    //Generic information
    public string unitName;
    public int unitLevel;

    //Enemy Type && Spawn Rate
    public enum type {SWORDSMAN, SPEARMAN, WIZARD, BOSS }
    public enum rarity {COMMON, UNCOMMON, RARE, SUPERRARE }

    public type enemyType;
    public rarity enemyRarity;

    //Enemy Health Point && Action Point
    public int maxHP;
    public int currentHP;
    public int maxAP;
    public int currentAP;

    //Enemy Stats Modifier
    [Header("Enemy Stats")]
    public int attackStat;
    public int defendStat;
    public int speedStat;
    public int recoveryStat;
    public int costAP;
}
