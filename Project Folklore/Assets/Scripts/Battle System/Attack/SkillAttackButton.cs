using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackButton : MonoBehaviour
{
    public BaseAttack skillToPerform;

    public void CastSkillAttack()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input3(skillToPerform);
    } 
}
