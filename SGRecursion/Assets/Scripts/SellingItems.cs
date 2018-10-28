using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingItems : MonoBehaviour {

    public Item[] itemsToSell = new Item[2];

    public Item GetRandomObject()
    {
        int rand = Random.Range(0, itemsToSell.Length);
        return itemsToSell[rand];
    }

    public Item GetSpecificObject(int id)
    {
        return null;
    }

}
