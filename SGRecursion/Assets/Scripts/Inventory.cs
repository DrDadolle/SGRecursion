using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour
{
    // synchronize the inventory of myself between all clients
    public SyncListInt listOfItemsID = new SyncListInt();

    public Transform dropPoint;
    public const int numItemSlots = 2;

    // Slots
    private GameObject[] itemSlots = new GameObject[numItemSlots];
    public ItemSlot[] allItemSlots = new ItemSlot[numItemSlots];

    private void Awake()
    {
        itemSlots = GameObject.FindGameObjectsWithTag("ItemSlot");
        allItemSlots[0] = itemSlots[0].GetComponent<ItemSlot>();
        allItemSlots[1] = itemSlots[1].GetComponent<ItemSlot>();
    }

    public void AddItemAction(int itemID)
    {
        if (allItemSlots[0].theItem == null)
        {
            allItemSlots[0].theItem = ItemManager.getItemById(itemID);
        }
        else if (allItemSlots[1].theItem == null)
        {
            allItemSlots[1].theItem = ItemManager.getItemById(itemID);
        }
        else
        {
            return;
        }

        CmdTellServerIGotANewItem(itemID);
        UpdateItemSlots();
    }

    [Command]
    private void CmdTellServerIGotANewItem(int itemID)
    {
        listOfItemsID.Add(itemID);
    }

    public void RemoveItemAction(int itemID)
    {
        if (listOfItemsID.Count > 0)
        {
            // CHECKS
            if (allItemSlots[0].theItem != null && allItemSlots[0].theItem.id == itemID)
            {
                allItemSlots[0].theItem = null;
            }
            else if (allItemSlots[1].theItem != null && allItemSlots[1].theItem.id == itemID)
            {
                allItemSlots[1].theItem = null;
            }
            else
            {
                return;
            }
        }
        CmdTellServerIRemovedAnItem(itemID);
        UpdateItemSlots();
    }

    [Command]
    private void CmdTellServerIRemovedAnItem(int itemID)
    {

        listOfItemsID.Remove(itemID);
    }


    public void DropItemAction(int index)
    {
        if (listOfItemsID.Count > 0)
        {
            // CHECKS
            if (allItemSlots[index].theItem != null)
            {
                Item it = allItemSlots[index].theItem;
                allItemSlots[index].theItem = null;
                //Spawn item
                CmdDropItem(it.id);
                UpdateItemSlots();
            }
        }
    }

    [Command]
    private void CmdDropItem(int id)
    {
        DropItem(id);
        listOfItemsID.Remove(id);
    }

    // Sub method
    private void DropItem(int itemID)
    {
        var dropItem = (GameObject)Instantiate(
           ItemManager.getItemById(itemID).itemObject,
           dropPoint);

        dropItem.transform.parent = null;

        NetworkServer.Spawn(dropItem);
    }

    private void UpdateItemSlots()
    {
        foreach(ItemSlot itslot in allItemSlots)
        {
            if(itslot.theItem == null)
            {
                GameObject img = itslot.transform.Find("Item Image").gameObject;
                img.SetActive(false);
                img.GetComponent<Image>().sprite = null;
            } else
            {
                Item it = itslot.theItem;
                GameObject img = itslot.transform.Find("Item Image").gameObject;
                img.SetActive(true);
                img.GetComponent<Image>().sprite = it.sprite;
            }
        }
    }


    public bool HasFreeSpace()
    {
        return listOfItemsID.Count < numItemSlots;
    }

    public bool HasItemById(int id)
    {
        return listOfItemsID.Contains(id);
    }


}