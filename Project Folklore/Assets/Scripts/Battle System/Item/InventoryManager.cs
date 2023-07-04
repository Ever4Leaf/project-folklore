using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<ItemBase> itemList = new List<ItemBase>();

    public Transform itemContent;
    public GameObject inventoryItem;

    public InventoryItemController[] inventoryItems;

    private void Awake()
    {
        //check if instance exist
        if (instance == null)
        {
            //if not then set instance to this
            instance = this;
        }
        //if there is but not this instance
        else if (instance != this)
        {
            //then destroy it
            Destroy(gameObject);
        }
        //set this instance to be not destroyable
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        itemContent = GameObject.Find("Canvas").transform.Find("Panel Inventory").transform.Find("Inventory Scroll View").transform.Find("Viewport Inventory").transform.Find("Content Inventory");

        ListItems();
    }

    public void AddItem(ItemBase item)
    {
        itemList.Add(item);
    }

    public void RemoveItem(ItemBase item)
    {
        itemList.Remove(item);
    }

    public void ListItems()
    {
        //Clean
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in itemList)
        {
            GameObject obj = Instantiate(inventoryItem, itemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.itemIcon;
        }

        //SetInventoryItems();
    }

    public void SetInventoryItems()
    {
        inventoryItems = itemContent.GetComponentsInChildren<InventoryItemController>();

        for (int i = 0; i < itemList.Count; i++)
        {
            inventoryItems[i].AddItem(itemList[i]);
        } 
    } 
}
