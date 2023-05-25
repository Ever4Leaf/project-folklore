using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntityClass
{
    //Generic information
    [Header("Unit Informations")]
    public string unitName;
    public int unitLevel;

    //Health Point 
    [Header("Health Point")]
    public float maxHP;
    public float currentHP;

    //Stats Modifier
    [Header("Stat Modifier")]
    public float attackStat;
    public float defendStat;
    public float speedStat;

    //Attack List
    [Header("Attacks List")]
    public List<BaseAttack> attackList = new List<BaseAttack>();
}
