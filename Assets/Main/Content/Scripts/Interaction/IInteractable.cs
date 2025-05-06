using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// The base interface for all interactable things
    /// </summary>
    public void OnInteract() {

    } // when we interact with anything
    public void OnHover() {

    } // when we hover over anything

    public bool CanInteract() {
        return true;
    }
    
    public int WhatInteractionType() {
        return 0;
    } // 0 for Interact, 1 for PickUp, 2 for Use, 3 for Open
}
