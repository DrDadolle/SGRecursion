using UnityEngine;
using UnityEngine.Networking;

public class SinglePlayerController : MonoBehaviour
{

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
        //tm = GameObject.FindGameObjectWithTag("TimeMachine").GetComponent<TimeMachine>();
        inv = gameObject.GetComponentInChildren<Inventory>();
    }


    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 200.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);


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




}
