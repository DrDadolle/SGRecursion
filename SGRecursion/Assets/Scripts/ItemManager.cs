using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    // For now : contains only a list of all items
    private static Dictionary<int, Item> allExistingItems = new Dictionary<int, Item>();

    public Item[] itemTest = new Item[2];

    private void Awake()
    {
        foreach (Item it in itemTest)
        {
            allExistingItems.Add(it.id, it);
        }
      
    }

    public static Item getItemById(int id)
    {
        return allExistingItems[id];
    }

    public static int getItemIdFromGO(GameObject go)
    {
        foreach(Item it in allExistingItems.Values)
        {
            if((it.itemObject.name + "(Clone)").Equals(go.name)){
                return it.id;
            }
        }

        return -1;
    }
}
