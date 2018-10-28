using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour
{
    // synchronize the inventory of myself between all clients
    public SyncListInt listOfItemsID = new SyncListInt();

    public Transform dropPoint;


    public Item[] items = new Item[numItemSlots];
    public const int numItemSlots = 2;

    public ItemManager itemManager;

    private GameObject[] itemSlots = new GameObject[numItemSlots];

    private void Awake()
    {
        itemManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ItemManager>();
        itemSlots = GameObject.FindGameObjectsWithTag("ItemSlot");
    }

    [Command]
    void CmdTellServerIGotANewItem(int itemID)
    {
        listOfItemsID.Add(itemID);
    }

    [Command]
    void CmdTellServerIRemovedAnItem(int itemID)
    {
        listOfItemsID.Remove(itemID);
    }


    // Utility method
    public bool HasItemById(int itemId)
    {
        foreach (Item it in items)
        {
            if (it != null && it.id == itemId)
            {
                return true;
            }
        }
        return false;

    }

    // Utility method
    public Item GetItemById(int itemId)
    {
        return itemManager.allExistingItems[itemId];

    }

    // Utility method
    public int GetIndexOfItemById(int itemId)
    {
        int count = -1;
        foreach(Item it in items)
        {
            count++;
            if (it != null && it.id == itemId)
            {
                break; 
            }
        }

        return count;
    }

    // Utility Method
    public bool HasFreeSpace()
    {
        foreach(Item it in items)
        {
            if(it == null)
            {
                return true;
            }
        }
        return false;
    }

    // Utility Method
    public bool AddItem(GameObject go)
    {
        foreach(Item it in itemManager.allExistingItems.Values)
        {
            if ((it.itemObject.name + "(Clone)").Equals(go.name))
            {
                AddItem(it.id);
                return true;
            }
        }
        return false;
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
                itemSlot.GetComponent<Image>().sprite = itemToAdd.sprite;

                // to synchronize with the server
                CmdTellServerIGotANewItem(itemID);

                return;
            }
        }


    }

    // Drop the number index item
    public void DropItem(int index)
    {
        Item itemToRemove = items[index];

        if (itemToRemove == null)
        {
            Debug.Log("Invalid id for item");
            return;
        }

        // Security check useless
        if (!listOfItemsID.Contains(itemToRemove.id))
        {
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToRemove)
            {

                // We need to add the display to the inventory
                GameObject itemSlot = itemSlots[i].transform.Find("Item Image").gameObject;
                itemSlot.SetActive(false);
                itemSlot.GetComponent<Image>().sprite = null;

                CmdDropItem(itemToRemove.id);

                // to synchronize with the server
                CmdTellServerIRemovedAnItem(itemToRemove.id);

                items[i] = null;

                return;
            }
        }
    }

    [Command]
    void CmdDropItem(int itemID)
    {
       
        // Create the item from the item prefab
        var itemToDrop = (GameObject)Instantiate(
             itemManager.allExistingItems[itemID].itemObject,
           dropPoint);

        // Spawn the item dropped on the Clients
        itemToDrop.transform.parent = null;

        NetworkServer.Spawn(itemToDrop);
    }

    // Drop the number index item
    public void RemoveItem(int index)
    {
        Item itemToRemove = items[index];

        if (itemToRemove == null)
        {
            Debug.Log("Invalid id for item");
            return;
        }

        // Security check useless
        if (!listOfItemsID.Contains(itemToRemove.id))
        {
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

                // to synchronize with the server
                CmdTellServerIRemovedAnItem(itemToRemove.id);

                return;
            }
        }
    }

}