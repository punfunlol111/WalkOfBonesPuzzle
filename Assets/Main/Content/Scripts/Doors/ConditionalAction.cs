using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConditionalAction : MonoBehaviour
{
    /// <summary>
    /// this script makes something happen once all the conditions are met
    /// </summary>
    /// 
    private bool triggered = false; 
    [SerializeField] private bool[] conditions; // list of conditions
    public UnityEvent onConditionsMetEvent; // the event gets triggered once all the conditions are true

    void Update()
    {
        if (!triggered) { // if we have not triggered yet keep chekcing
            foreach (bool condition in conditions) {
                if (condition == false) {
                    return; // if even one of htem is false return
                }
            }// else trigger and call the event
            triggered = true;
            onConditionsMetEvent?.Invoke();
        }
    }
    public void SetConditionTrue(int index) {
        conditions[index] = true;
    } // sets the condition to true; 
    public void SetConditionFalse(int index) {
        conditions[index] = false;

    } // sets the condition to false
}
