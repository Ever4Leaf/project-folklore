using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerBase :BaseEntityClass
{
    //player Animator
    [Header("Player Animator")]
    public Animator playerAnimator;

    //Player Action Point
    [Header("Action Point")]
    public float maxAP;
    public float currentAP;

    //Player Skill Set
    [Header("Skill Set")]
    public List<BaseAttack> skillList = new List<BaseAttack>();
}
