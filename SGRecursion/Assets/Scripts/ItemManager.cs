using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    // For now : contains only a list of all items
    public Dictionary<int, Item> allExistingItems = new Dictionary<int, Item>();

    public Item[] itemTest = new Item[2];

    private void Awake()
    {
        foreach (Item it in itemTest)
        {
            allExistingItems.Add(it.id, it);
        }
      
    }
}
