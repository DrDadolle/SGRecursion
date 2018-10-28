using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Item[] items = new Item[numItemSlots];
    public const int numItemSlots = 2;

    public ItemManager itemManager;

    
    public void AddItem(int itemID)
    {
        Item itemToAdd = itemManager.allExistingItems[itemID];

        if(itemToAdd == null)
        {
            Debug.Log("Invalid id for item");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = itemToAdd;

                // We need to add the display to the inventory
                GameObject itemSlot = gameObject.transform.GetChild(i).gameObject.transform.Find("Item Image").gameObject;
                itemSlot.SetActive(true);
                itemSlot.GetComponent<Image>().sprite = itemToAdd.sprite;

                return;
            }
        }
    }
    public void RemoveItem(int itemID)
    {

        Item itemToRemove = itemManager.allExistingItems[itemID];

        if (itemToRemove == null)
        {
            Debug.Log("Invalid id for item");
            return;
        }


        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToRemove)
            {
                items[i] = null;

                // We need to add the display to the inventory
                GameObject itemSlot = gameObject.transform.GetChild(i).gameObject.transform.Find("Item Image").gameObject;
                itemSlot.SetActive(false);
                itemSlot.GetComponent<Image>().sprite = null;

                return;
            }
        }
    }
}