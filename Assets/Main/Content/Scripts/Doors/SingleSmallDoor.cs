using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingleSmallDoor : MonoBehaviour, IInteractable
{
    private Animator animator; // the refrance to the animator
    private const string OPEN_DOOR = "Open";
    private const string CLOSE_DOOR = "Close";
    private const string OPEN_DIRECTION = "Direction"; // keys for the aniumator

    [SerializeField] private bool rightSideHinge; // whre the hige is, important for doors that the player does not open, so we can swing open the right wat
    [SerializeField] private bool canBeOpenedByHand; // can we open it by hand

    private bool doorOpen = false; // is the door open

    public static event Action evt_DoorOpen;  // event door open
    public static event Action evt_DoorClose; // event door close

    void Start()
    {
        animator = GetComponent<Animator>(); // grab the aniamor
    }
    public void OpenDoor(int direction) { // when we open the door
        if (!rightSideHinge)
            direction = -direction; // if we are not right side hinged invert
        animator.SetInteger(OPEN_DIRECTION, direction); // set the animator
        animator.SetTrigger(OPEN_DOOR);
        doorOpen = true; // we are open
        evt_DoorOpen?.Invoke(); // event trigger
    }
    public void CloseDoor() {
        animator.SetTrigger(CLOSE_DOOR); // trigger aniamtor
        doorOpen=false; // close door
        evt_DoorClose?.Invoke(); // event trigger
    }

    public void OnInteract() {
        if (!doorOpen) { // if the door is not open, if it is we close the door
            float dotDoorToPlayer = Vector3.Dot(PlayerController.Instance.transform.forward, transform.forward);
            if(dotDoorToPlayer >= 0) { // we get the dot profuct between the door and the player, if its bigger then one swing back, if smaller then swing forward. this is so we alwys open the door from the right side, reguardless of where the player is standing
                OpenDoor(1);
            } else {
                OpenDoor(-1);
            }
        } else {
            CloseDoor();
        }
        
    }
    public bool CanInteract() {
        return canBeOpenedByHand;
    } // we can only interact ourselves if its open by hand, otehrwise we need an event call from somewhere else

    public int WhatInteractionType() {
        return 3;
    } // 0 for Interact, 1 for PickUp, 2 for Use, 3 for Open
}
