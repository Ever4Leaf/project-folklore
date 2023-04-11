using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitModifier : MonoBehaviour
{
    [Header("Unit Information")]
    //Generic information
    public string unitName;
    public int unitLevel;

    //Unit Health Point && Action Point
    public int maxHP;
    public int currentHP;
    public int maxAP;
    public int currentAP;

    //Unit Stats Modifier
    [Header("Unit Modifiable Stats")]
    public int attackStat;
    public int defendStat;
    public int speedStat;
    public int recoveryStat;
    public int costAP;

    /*[Header("Unit Un-Modifiable Stats")]
    [SerializeField] private float moveMultiplier = 1.0f;
    [SerializeField] private int moveRNG;

    [Header("Unit Damage Calculatioin")]
    [SerializeField] private float damageValue;
    public int finalDamageCalculation;

    /*Status effect
    public int poisonEffect;
    public int poisonTurnEffect;
    public int bleedEffect;
    public int bleedTurnEffect;

    //Referencing Unit Modifier
    unitModifier playerUnit;
    unitModifier enemyUnit;

    public void damageGiven()
    {
        moveRNG = Random.Range(1, 5);

        //Damage Calculation
        damageValue = moveMultiplier*((1+(0.1f*playerUnit.attackStat*moveRNG))-(enemyUnit.defendStat/enemyUnit.defendStat+100));
        finalDamageCalculation = Mathf.CeilToInt(damageValue);
        Debug.Log("Dmg value in float = " + damageValue);
        Debug.Log("Dmg value in integer = " + finalDamageCalculation);
    }*/

    public bool takeDamage(int finalDamage)
    {
        currentHP -= finalDamage;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void recoverHP(int recoverAmount)
    {
        currentHP += recoverAmount;

        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public bool onMoveUse(int costAP)
    {
        if (currentAP >= costAP)
        {
            currentAP -= costAP;
            return true;
        }
        else
            return false;
    }
}
