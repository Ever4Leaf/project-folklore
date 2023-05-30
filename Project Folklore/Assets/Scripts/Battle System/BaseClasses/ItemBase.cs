using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemBase
{
    [Header("Item Informations")]
    public string itemName;

    public enum type { HP_RECOVERY, AP_RECOVERY }
    public type itemType;

    public float recoveryPoint;
}
