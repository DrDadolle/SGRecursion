﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    // For shooting
    public GameObject bulletPrefab;
    public Transform bulletSpawn;


    // For shopping
    public bool canBuyItem = false;
    public Item buyableItem;

    // For timeMachine
    public TimeMachine tm;
    public bool isNearTimeMachine = false;
    public int useableItemID;

    public Inventory inv;

    private void Start()
    {
        tm = GameObject.FindGameObjectWithTag("TimeMachine").GetComponent<TimeMachine>();
        inv = gameObject.GetComponentInChildren<Inventory>();
    }


    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 200.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        //FIRE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }

        // GrabItem Inventory
        if (Input.GetKeyDown(KeyCode.I) && canBuyItem)
        {
            inv.AddItemAction(buyableItem.id);
        }

        // Time Machine
        if (isNearTimeMachine)
        {
            // Upgrade the TM
            if (tm.canBeUpgraded && Input.GetKeyDown(KeyCode.T) && useableItemID != -1)
            {
                inv.RemoveItemAction(useableItemID);
                useableItemID = -1;
                CmdUpgradeTimeMachine();
            } 

            // Use the TM
            if (tm.canBeUsed && Input.GetKeyDown(KeyCode.Y))
            {
                //TODO : send DMail
                Debug.Log("Sending D-mail");
            }
        }

        // Remove object from inventory slot 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
             inv.DropItemAction(0);
        }

        // Remove object from inventory slot 2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
             inv.DropItemAction(1);
        }


    }

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    [Command]
    public void CmdUpgradeTimeMachine()
    {
        tm.UpgradeMachine();
        Debug.Log("TM UPGRADE");
    }

    [Command]
    public void CmdDeleteObject(NetworkInstanceId objectId)
    {
        var myObject = NetworkServer.FindLocalObject(objectId);
        NetworkServer.Destroy(myObject);
    }



    }
