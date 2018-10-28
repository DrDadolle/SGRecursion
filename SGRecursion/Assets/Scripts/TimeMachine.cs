using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TimeMachine : NetworkBehaviour {

    public Item[] possibleUseableItems = new Item[2];

    [SyncVar]
    public int requiredItemIdToUse = -1;

    [SyncVar]
    public bool canBeUpgraded;

    [SyncVar]
    public bool canBeUsed;

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
        canBeUpgraded = true;
    }

    public void UpgradeMachine()
    {
        TimeMachineLevel = TimeMachineLevel + 1;
        requiredItemIdToUse = -1;
        canBeUsed = true;
        canBeUpgraded = false;
    }

}
