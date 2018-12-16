using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimationDoor : MonoBehaviour {

    Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void PauseAnimationEvent()
    {
        anim.enabled = false;
    }
}
