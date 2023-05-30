using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurnHandler 
{
    public string attackerName; //Attacker Name 
    public string attackerType; //Player OR Enemy
    public GameObject attackerGO; //Attacker Game Object
    public GameObject attackTarget; //Attacker Target

    //what attack is performed
    public BaseAttack usedAttack;
}
