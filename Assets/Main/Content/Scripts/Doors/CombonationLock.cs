using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CombonationLock : MonoBehaviour
{
    /// <summary>
    /// This class is for a basic combonation lock that triggers a uhnity event when the lock is opened
    /// </summary>
    public UnityEvent eventOnUnlock; // the event that happens when the lock is triggered

    [SerializeField] private TMP_Text[] text; // a list of the texts so we can set them to x value
    
    private bool locked = true; // is the lock locked ? 
    
    [SerializeField] private int[] combonation; // the secret combonation
    private int[] currentLockValue; // the current value of the lock
    private void Start() {
        currentLockValue = new int[combonation.Length]; // set the length of the current lock to the legnth of the combonastion which also sets all the numbers to zero
        SetTextValue(); // set the text to zero
    }
    public void AddNumber(int slot) {
        if (!locked) // we cant interact with it if the lock is open
            return;
        int value = int.Parse(text[slot].text); // parse the current value into an int so we can do some maths
        value = value+1 > 9 ? 0 : value+1; // check if the value plus 1 is bigger then 9 and if so loop it back to zero
        currentLockValue[slot] = value; // set the value of the current lock state
        SetTextValue(); // sets the value of the text
    } // function for adding a number to the lock in slot x set the by the indevidual buttons with the interactable action script

    public void RemoveNumber(int slot) {
        if (!locked)
            return;
        int value = int.Parse(text[slot].text);
        value = value - 1 < 0 ? 9 : value - 1;
        currentLockValue[slot] = value;
        SetTextValue();
    } // function for removing a number from the lock. works the same as {AddNumber} see it for more detail

    public void CheckIfUnlocked() {
        for (int i = 0; i < combonation.Length; i++) {
            if (combonation[i] == currentLockValue[i]) {

            } else {
                return;
            }
        } // check to see if the lock is locked by looping through the comboation value and comparing it to the lock value
        locked = false;
        eventOnUnlock.Invoke(); // we unlock here since all the condition where met
        SetTextValue(); // set the text one more time to the solved text
    }
    private void SetTextValue() {
        if (!locked) {
            string openText = "NICE";
            for (int i = 0; i < text.Length; i++) { 
                text[i].text = openText[i].ToString();
            }
            return; // if the door is open it reads the open Text variable and sets the vlaue of the combonation lock to display it
        } 
        for (int i = 0; i < text.Length; i++) {
            text[i].text = currentLockValue[i].ToString();
        } // displayes the current value of the lock.
    }
}
