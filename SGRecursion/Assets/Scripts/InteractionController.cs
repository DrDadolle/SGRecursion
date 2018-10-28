using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    PlayerController playerController;

    private void Awake()
    {
        playerController  = gameObject.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Shop")) {

            Debug.Log("Can press I to get an item !");
            playerController.canBuyItem = true;
            playerController.buyableItem = other.gameObject.GetComponent<SellingItems>().GetRandomObject();

        } else if (other.gameObject.tag.Equals("TimeMachine"))
        {
            Debug.Log("Entering the time machine");
            TimeMachine tm = other.gameObject.GetComponent<TimeMachine>();
            
            if (playerController.GetComponent<Inventory>().HasItemById(tm.requiredItemIdToUse))
            {
                playerController.useableItemID = tm.requiredItemIdToUse; 
                playerController.canUpgradeTimeMachine = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Shop"))
        {
            Debug.Log("exiting the shop");
            gameObject.GetComponent<PlayerController>().canBuyItem = false;
        }
        else if (other.gameObject.tag.Equals("TimeMachine"))
        {
            Debug.Log("Exiting the time machine");
            playerController.canUpgradeTimeMachine = false;
        }
    }
}
