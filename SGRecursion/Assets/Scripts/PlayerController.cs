using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public Transform camPlace;

    public bool canBuyItem = false;
    public Item buyableItem;


    public override void OnStartLocalPlayer()
    {
       GetComponent<MeshRenderer>().material.color = Color.blue;
       Camera.main.transform.parent = camPlace;
       Camera.main.transform.position = camPlace.position;
       GameObject hudPlayer = GameObject.FindGameObjectWithTag("HUDPlayer");
       hudPlayer.transform.parent = this.transform;

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

        // Test Inventory
        if (Input.GetKeyDown(KeyCode.I) && canBuyItem)
        {
            CmdAddItem(buyableItem.id);
        }

        // Test Inventory
        if (Input.GetKeyDown(KeyCode.J))
        {
            CmdRemoveItem(buyableItem.id);
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
    void CmdAddItem(int itemID)
    {
        gameObject.GetComponentInChildren<Inventory>().AddItem(itemID);
    }

    [Command]
    void CmdRemoveItem(int itemID)
    {
        gameObject.GetComponentInChildren<Inventory>().RemoveItem(itemID);
    }
}
