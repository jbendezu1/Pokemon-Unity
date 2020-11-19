using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Items> itemList;

    public Inventory()
    {
        itemList = new List<Items>();
    }

    public void AddItem(Items item)
    {
        itemList.Add(item);
    }

    public List<Items> GetItemList()
    {
        return itemList;
    }
}
