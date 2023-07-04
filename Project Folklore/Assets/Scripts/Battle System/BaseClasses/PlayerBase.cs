using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerBase : BaseEntityClass
{
    [Header("Player EXP")]
    public float maxEXP;
    public float currentEXP;

    [Header("Action Point")]
    public float maxAP;
    public float currentAP;

    [Header("Skill Set")]
    public List<MovesetBase> skillList = new List<MovesetBase>();
}
