using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemController : MonoBehaviour
{
    ItemBase item;

    public void RemoveItem()
    {
        InventoryManager.instance.RemoveItem(item);

        Destroy(gameObject);
    }

    public void AddItem(ItemBase newItem)
    {
        item = newItem;
    }
}
