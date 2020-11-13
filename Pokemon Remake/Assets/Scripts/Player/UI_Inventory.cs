using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotContainer = itemSlotContainer.Find("itemSlotTemplate");
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }
    
    private void RefreshInventoryItems()
    {
        foreach (Items item in inventory.GetItemList())
        {

        }
    }
}
