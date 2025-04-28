using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Item : MonoBehaviour , IInteractable{
   
    // this class will be for all the items, that can be interacted with and be picked up
    private Rigidbody rb;
    private Collider itemCollider;

    [SerializeField] private bool destroyOnUse = true; // destroy the item when used, like a key for example
    [SerializeField] private int itemID; // this id is not diffent for all items. its 0 for most. its used to track keys and other items needed to open doors and such. these will be manually set to work with doors and sucj in the inspector
    
    public int ItemID {  get { return itemID; } } // gets the id to read. we dont want to be able to set it in code.
    private int itemState = 0; // 0 means in the world and 1 means in the hand 2 means its being thown

    // -- Events --
    public static event Action<GameObject> evt_OnCollideAfterBeingThrown;
    public static event Action<GameObject> evt_OnPickUp;
    public static event Action<GameObject> evt_OnPutDown;
    // -------------

    void Start() {
        rb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
    }
    
    void Update() {
        UpdateItemState(); // upades the item state all the time
    }
    public bool CanInteract() {
        return itemState == 0; // if the item state is on the ground
    }
    private void UpdateItemState() {
        switch (itemState) {
            case 0: // on ground
                rb.isKinematic = false; 
                itemCollider.isTrigger = false;
                rb.interpolation = RigidbodyInterpolation.None;
                break;
            case 1: // in hand
                rb.isKinematic = true;
                itemCollider.isTrigger = true;
                rb.interpolation = RigidbodyInterpolation.None;
                SetPositionRotationInHand();
                break;
            case 2: // being thrown
                rb.isKinematic = false;
                itemCollider.isTrigger = false;
                rb.interpolation = RigidbodyInterpolation.Interpolate;

                rb.AddRelativeTorque(transform.right * 10);
                break;
        }
    } // this function updates the items state, so it knows when its being carried or thrown
    private Vector3 smoothVel; // a refrance variable for the smooth damp function
    private void SetPositionRotationInHand() {
        if(itemState == 1) {
            transform.rotation = Quaternion.Lerp(transform.rotation, PlayerController.Instance.transform.rotation, .1f);
            transform.position = Vector3.SmoothDamp(transform.position, PlayerController.Instance.PlayerCamera.ScreenToWorldPoint(new Vector3(1500, 300, 1)),ref smoothVel,0.05f);
        }
    } // makes sure when the item is in the hand it moves to corrent positiojn
    #region Actions
    public void OnInteract() {
        PickUp();
    }
    public void OnHover() {
        // hover 
    }
    public void PickUp() {
        itemState = 1; // item state in hand
        PlayerController.Instance.HeldItem = gameObject.GetComponent<Item>(); // set the held item of the player to this
        evt_OnPickUp?.Invoke(gameObject); // invoke picked up
    } // for when we pick up the item
    public void Droped(Vector3 where) {
        transform.position = where; // put it at the location
        itemState = 0; // item state 0, on ground
        PlayerController.Instance.HeldItem = null; // set our held item to null
        evt_OnPutDown?.Invoke(gameObject); // item put down 
    } // when we drop the item
    public void Thrown(Vector3 direction, float velocity) {
        itemState = 2; // set the state   
        PlayerController.Instance.HeldItem = null; // unparent the item
        
        UpdateItemState(); // we make an extra update here to make sure that the rigid body can move and the collider can collide

        // make sure the throw starts from the center of ther screen
        Vector3 initialPosition = PlayerController.Instance.PlayerCamera.ViewportToWorldPoint(new Vector3(.5f, .5f, .8f));
        rb.Move(initialPosition, Quaternion.identity);
        rb.AddForce(direction * velocity, ForceMode.Impulse); // add the 

    }
    public void Used() {
        transform.position = new Vector3(1000, 1000, 1000); // move it out of the wat
        itemState = 0; // chenge the state
        PlayerController.Instance.HeldItem = null; // set item of player to null
        if (destroyOnUse) { // if we destory on use then que the item for destruction
            Destroy(gameObject, 1);
            Destroy(this);
        } else {

        }
    } // when you use a key or something
    #endregion
    private void OnCollisionEnter(Collision collision) {
        if(itemState == 2) { // when it hits something after being thrown make a boop sound and put it back to item state 0
            itemState = 0;
            evt_OnCollideAfterBeingThrown?.Invoke(gameObject);
        } // after being thrown the item enters the Thrown state. in that state it cant be interacted with. so when the item is in the throw state and collides with something it enters back into the On the gorund state and can be interacted with once again.
    }
    public int WhatInteractionType() {
        return 1;
    } // 0 for Interact, 1 for PickUp, 2 for Use

    public int GetCurrentItemState() => itemState; // gets the current itemstate and returns it
}
