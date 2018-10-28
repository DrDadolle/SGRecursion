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

    public void DropItem(int itemID)
    {
        // Security check useless
        if (!listOfItemsID.Contains(itemID))
        {
            return;
        }


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

                // We need to add the display to the inventory
                GameObject itemSlot = itemSlots[i].transform.Find("Item Image").gameObject;
                itemSlot.SetActive(false);
                itemSlot.GetComponent<Image>().sprite = null;

                Debug.Log("TRYING TO DROP!");
                Debug.Log(itemToRemove);
                Debug.Log(itemToRemove.id);
                Debug.Log(itemToRemove.sprite);
                Debug.Log(itemToRemove.itemObject);
                CmdDropItem(itemID);

                // to synchronize with the server
                CmdTellServerIRemovedAnItem(itemID);

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

        // Add velocity to the item
        itemToDrop.GetComponent<Rigidbody>().velocity = itemToDrop.transform.forward;

        NetworkServer.Spawn(itemToDrop);
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

                // to synchronize with the server
                CmdTellServerIRemovedAnItem(itemID);

                return;
            }
        }
    }
}