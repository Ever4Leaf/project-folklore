using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyBase : BaseEntityClass
{
    //Enemy Type && Spawn Rate
    public enum type {SWORDSMAN, SPEARMAN, WIZARD, BOSS }
    public enum rarity {COMMON, UNCOMMON, RARE, SUPERRARE }

    [Header("Enemy Animator")]
    //public Animator enemyAnimator;   

    [Header("Enemy Type & Rarity")]
    public type enemyType;
    public rarity enemyRarity;
}
