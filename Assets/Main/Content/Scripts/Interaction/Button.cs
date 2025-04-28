using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Button : MonoBehaviour, IInteractable
{ // Verry simple sctipt has a unity event that gets fired when iteracted with and also handles the button animation


    public static event Action evt_ButtonPress;

    private Animator animator; // aniamot
    public UnityEvent buttonPressedEvent; // the event that happens when the button is presseed

    // 1 for if you want the button to only be able to be pressed once otherwise 
    [SerializeField] private int buttonPressType; // press type does it come back up or stay held down
    private bool hasBeenPressed = false; // has it been presses? relevent if we want it to stay down

    private const string TRIGGER = "Press"; // aniamtor refrance to the press actuiin
    private const string BUTTON_PRESS_TYPE = "Type"; // animator refrance to the press type
    void Start()
    {
        animator = GetComponentInParent<Animator>(); // grab animator
    }
    public void OnInteract() {
        animator.SetInteger(BUTTON_PRESS_TYPE, buttonPressType); // sets the type of press
        animator.SetTrigger(TRIGGER); // sets the trigger of pressed in the animator
        hasBeenPressed = true; // we have pressed the bnutton nows
        buttonPressedEvent?.Invoke(); // invokes the event for what we want the button to do
        evt_ButtonPress?.Invoke();
    }
    public bool CanInteract() {
        if(buttonPressType == 1 && hasBeenPressed) // if the id = 1 and we have pressed the button before we want press it agin
            return false;
        return true;
    }
}
