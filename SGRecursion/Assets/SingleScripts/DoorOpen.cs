using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour {

    Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("DoorOpen");
    }

    private void OnTriggerExit(Collider other)
    {
        //TODO
        anim.enabled = true;
    }

}
