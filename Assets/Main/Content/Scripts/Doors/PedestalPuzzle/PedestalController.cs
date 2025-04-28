using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PedestalController : MonoBehaviour
{
    public UnityEvent onUnlockEvent; // the event that happens when the door is unlocked

    private bool opned = false;
    [SerializeField] private Pedestal[] pedestals; // the refrance to the pedestals
    [SerializeField] private int[] combonation; // the target valye to open door or smthg.
    private int[] currentValue; // current valye of the pedestals
    void Start()
    {
        if (pedestals.Length != combonation.Length)
            Debug.LogError("Combonation needs to mach length of pedestals");
        currentValue = new int[pedestals.Length]; // new array depending on the size
    }

    void Update()
    {
        if (!opned) { // if we have not finished the pedestal puzzle keep checking and seeting the lock
            SetCurrentValue(); 
            CheckLock();
        }
    }

    private void SetCurrentValue() {
        for (int i = 0; i < pedestals.Length; i++) {
            currentValue[i] = pedestals[i].GetPedestalState();
        } // loops through all the pedestals and attributes them a value
    } // sets the current value of the lock to what we set in the inspector

    private void CheckLock() {
        for(int i = 0;i < pedestals.Length; i++) {
            if (currentValue[i] != combonation[i])
                return;
        } // loops through all the pedestals and checks them 1 by 1
        onUnlockEvent?.Invoke(); // unlcoked
        opned = true; // we opened
    } // checks if the current comboation is the correct one
}
