using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InteractionController : MonoBehaviour
{
    PlayerController playerController;

    private void Awake()
    {
        playerController  = gameObject.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {

        switch (other.gameObject.tag)
        {
            case "Shop":
                Debug.Log("Can press I to get an item !");
                playerController.canBuyItem = true;
                playerController.buyableItem = other.gameObject.GetComponent<SellingItems>().GetRandomObject();
                break;
            case "TimeMachine":
                Debug.Log("Entering the time machine");
                playerController.isNearTimeMachine = true;
                TimeMachine tm = other.gameObject.GetComponent<TimeMachine>();

                if (playerController.GetComponent<Inventory>().HasItemById(tm.requiredItemIdToUse))
                {
                    playerController.useableItemID = tm.requiredItemIdToUse;
                }
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "PickupObject":
                if (Input.GetKeyDown(KeyCode.E) && playerController.inv.HasFreeSpace())
                {
                    Debug.Log("PICKING UP the GO : " + other.gameObject);
                    playerController.inv.AddItemAction(ItemManager.getItemIdFromGO(other.gameObject));
                    playerController.CmdDeleteObject(other.gameObject.GetComponent<NetworkIdentity>().netId);
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Shop":
                Debug.Log("exiting the shop");
                gameObject.GetComponent<PlayerController>().canBuyItem = false;
                break;
            case "TimeMachine":
                Debug.Log("Exiting the time machine");
                playerController.isNearTimeMachine = false;
                break;

        }
    }
}
