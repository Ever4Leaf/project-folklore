using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Moveset", menuName = "Moveset/Create New Moveset")]
public class MovesetBase : ScriptableObject
{
    public int movesetID;
    public string movesetName;
    public float movesetValue;
    public float movesetRecoverValue;
    public float movesetCost;

    public enum type { ATTACK, HEAL, BUFF, DEBUFF}
    public type movesetType;
}
