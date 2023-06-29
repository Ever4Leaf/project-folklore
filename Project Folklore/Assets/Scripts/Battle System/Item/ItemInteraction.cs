using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public ItemBase Item;

    public void PickUp()
    {
        InventoryManager.instance.AddItem(Item);
        Destroy(gameObject);
    }

    public void OnMouseDown()
    {
        PickUp();
    }
}
