using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour, IInteractable
{
    // this is the pedestal class it holds all the data related to the indevidula pedestal in the pedestal puzzle.
    // each pedestal will have 8 states. 

    private int currentState = 0; // what state the pedestal is in
    private void RotatePedestal() {
        transform.Rotate(new Vector3(0,45,0)); // rotates the pedestal
    }
    public void OnInteract() {
        RotatePedestal(); // calls to rotate when itneracted with

        currentState++; // adds to the state if its bigger then 7 aka the amount of roatations to make a full revolution it sets it back to 0
        if(currentState > 7) // if we roated a full circle loop back to 0
            currentState = 0;
    }
    public int GetPedestalState() => currentState; // to read the state from the controller
    public bool CanInteract() {
        return true;
    }

    public int WhatInteractionType() {
        return 0;
    } // 0 for Interact, 1 for PickUp, 2 for Use, 3 for Open
}
