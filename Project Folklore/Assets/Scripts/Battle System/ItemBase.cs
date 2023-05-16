using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemBase
{
    [Header("Item Informations")]
    //Generic information
    public string unitName;
    public int unitLevel;

    public enum type { HP_RECOVERY, AP_RECOVERY }
    public type itemType;

    //Item Properties
    public int maxHP;
    public int currentHP;
    public int maxAP;
    public int currentAP;
}
