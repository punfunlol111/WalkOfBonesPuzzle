using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class HoverOverInteractableGraphic : MonoBehaviour
{
    [SerializeField] private GameObject interactionGraphic; // the graphic that is shown

    private float timeTillGraphicDies = .2f; // time till graphic dies after we stop hivering, a little padding time more smooth

    private void Start() {
        PlayerController.Instance.evt_OnHover += OnHover; // any on hover will trigger this
    }

    private void OnHover(GameObject obj) {
        if (CheckIfItemInHand(obj) && obj.GetComponent<IInteractable>().WhatInteractionType() != 2)
            return; // if we have an item in out hand and we try to interact with another "Pick up" interaction it wont show the the graphic
        // We are checking that we dont show the graphic if we are holding an item. but we do show if the item is supposed to be used.
        SetInteractionGraphicText(obj.GetComponent<IInteractable>().WhatInteractionType()); // sets the text

        timeTillGraphicDies = .2f; // keeps the time a .2 so the grapic does not die while hovering
        interactionGraphic.SetActive(true); // set the graphic active
    }

    private void Update() { // tries to lower the time, only works if we are not hivering then kills the graphic
        if((timeTillGraphicDies-=1*Time.deltaTime) <= 0) {
            interactionGraphic.SetActive(false); // updates the 
        }
    }

    private bool CheckIfItemInHand(GameObject obj) {
        if (obj.GetComponent<Item>()) {
            if(PlayerController.Instance.HeldItem == null) {
                return false;
            }
            return true;
        }
        return false;
    }// Chekcs to see if the player already has an item in hand, and if it does it wont show the interact graqphic

    #region Interaction Graphic
    private static string[] interactionGraphicTextValues = { "Interact", "Pick Up", "Use", "Open" }; // different tpyes of interaction text

    private void SetInteractionGraphicText(int interactionId) {
        TMP_Text interactionText = interactionGraphic.GetComponentInChildren<TMP_Text>();
        interactionText.text = interactionGraphicTextValues[interactionId] + " E";
    } // sets the interaction text to the type we need
    #endregion
}
