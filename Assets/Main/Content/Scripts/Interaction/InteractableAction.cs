using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableAction : MonoBehaviour, IInteractable
{

    [SerializeField] private bool requiresItem; // do you need an item to be used to open it... like a lock
    [SerializeField] private int requiredItemId; // what id that item needs to have to be accepted asnd used
    [SerializeField] private bool canOnlyInteractOnce; // this is for things that acan be interacted with once;
    private bool hasBeenInteractedWith = false;
    public UnityEvent action; // action that happens when called

    public bool RequiresItem { get { return requiresItem; } } // public getter for needing an item


    public void OnInteract() {
        hasBeenInteractedWith = true;
        action?.Invoke(); // Invoke the action
    }
    public void OnHover() {

    }

    public bool CanInteract() {
        if(!requiresItem)
            return true;
        if (hasBeenInteractedWith && canOnlyInteractOnce)
            return false;
        return PlayerController.Instance.HeldItem?.ItemID == requiredItemId;
    } // checks if we can interact with the thing. if we dont need an item we can alwys, but if we do need an item we can only interact with the thing once we have the right item in hand.

    public int WhatInteractionType() {
        return requiresItem == true ? 2 : 0;
    } // 0 for Interact, 1 for PickUp, 2 for Use, 3 for Open
}
