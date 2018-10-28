using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour
{
    public Item[] items = new Item[numItemSlots];

    public const int numItemSlots = 2;

    public ItemManager itemManager;

    private GameObject[] itemSlots = new GameObject[numItemSlots];

    private void Awake()
    {
        itemManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ItemManager>();
        itemSlots = GameObject.FindGameObjectsWithTag("ItemSlot");
    }

    
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
                GameObject itemSlot = itemSlots[i].transform.Find("Item Image").gameObject;
                itemSlot.SetActive(true);
                Debug.Log(itemSlot);
                Debug.Log(itemSlot.GetComponent<Image>());
                Debug.Log(itemSlot.GetComponent<Image>().sprite);
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
                GameObject itemSlot = itemSlots[i].transform.Find("Item Image").gameObject;
                itemSlot.SetActive(false);
                itemSlot.GetComponent<Image>().sprite = null;

                return;
            }
        }
    }
}