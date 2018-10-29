//-------------------------------------
// Responsible for setting up the player.
// This includes adding/removing him correctly on the network.
//-------------------------------------
using UnityEngine;
using UnityEngine.Networking;
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    Camera sceneCamera;

    // Camera
    public Transform camPlace;


    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
        Camera.main.transform.parent = camPlace;
        Camera.main.transform.position = camPlace.position;
        GameObject hudPlayer = GameObject.FindGameObjectWithTag("HUDPlayer");
        hudPlayer.transform.parent = this.transform;

    }


    void Start()
    {
        // Disable components that should only be
        // active on the player that we control
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
    }
    // When we are destroyed
    void OnDisable()
    {

    }
}