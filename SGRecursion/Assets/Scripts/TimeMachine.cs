using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TimeMachine : NetworkBehaviour {

    public Item[] possibleUseableItems = new Item[2];

    [SyncVar]
    public int requiredItemIdToUse;

    [SyncVar]
    public int TimeMachineLevel = 0;

    public Item GetRandomObject()
    {
        int rand = Random.Range(0, possibleUseableItems.Length);
        return possibleUseableItems[rand];
    }

    private void Awake()
    {
        requiredItemIdToUse = GetRandomObject().id;
    }

}
