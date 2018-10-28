using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Shop")) {
            Debug.Log("Can press I to get an item !");
            PlayerController playerController = gameObject.GetComponent<PlayerController>();
            playerController.canBuyItem = true;
            playerController.buyableItem = other.gameObject.GetComponent<SellingItems>().GetRandomObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Shop"))
        {
            Debug.Log("exiting the shop");
            gameObject.GetComponent<PlayerController>().canBuyItem = false;
        }
    }
}
