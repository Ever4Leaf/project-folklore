using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyBase : BaseEntityClass
{
    //Enemy Type && Spawn Rate
    public enum Type {SWORDSMAN, SPEARMAN, WIZARD, BOSS }
    public enum Rarity {COMMON, UNCOMMON, RARE, SUPERRARE }

    [Header("Enemy Type & Rarity")]
    public Type enemyType;
    public Rarity enemyRarity;

    [Header("Enemy Give EXP")]
    public float dropEXP;
}
