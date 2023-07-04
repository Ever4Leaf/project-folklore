using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntityClass
{
    [Header("Char Animator")]
    public Animator charAnimator;

    [Header("Unit Informations")]
    public string unitName;
    public int unitLevel;

    [Header("Health Point")]
    public float maxHP;
    public float currentHP;

    [Header("Stat Modifier")]
    public float attackStat;
    public float defendStat;
    public float speedStat;

    [Header("Attacks List")]
    public List<MovesetBase> attackList = new List<MovesetBase>();
}
