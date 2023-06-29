using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class ItemBase : ScriptableObject
{
    public int itemId;
    public string itemName;
    public float itemValue;
    public Sprite itemIcon;

    public enum type { HEALING, STATUS_RECOVERY}
    public type itemType;
}
