using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class TouchAction : MonoBehaviour
{
    /// <summary>
    ///  This here is a general class for making htis happen when you tough them like lava or the end zoen
    /// </summary>
    public UnityEvent touchAction; // touch
    public UnityEvent exitAction; // exit

    private static float TIME_TILL_CAN_TOUCH = 1f; // how often touiches are registered
    private bool canTouch = true; // can we touch 
    private float timeTillCanTouch = 0; // time till we can touch again
    void Start()
    {
        
    }

    void Update()
    {
        if (!canTouch) {
            timeTillCanTouch += Time.deltaTime;
            if (timeTillCanTouch > TIME_TILL_CAN_TOUCH) {
                canTouch = true;
                timeTillCanTouch = 0;
            }
        } // timer counting down till when we can touch again
    }

    private void OnCollisionEnter(Collision collision) {
       if (collision.collider.GetComponent<PlayerController>() != null && canTouch) {
           touchAction?.Invoke();
           canTouch = false;
       }
    } // for collion enter
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>() != null && canTouch) {
            touchAction?.Invoke();
            canTouch = false;
        }
    } // for trigger enter for things that are not solid like the teleporting zone
    private void OnCollisionExit(Collision collision) {
        if (collision.collider.GetComponent<PlayerController>() != null && canTouch) {
            exitAction?.Invoke();
            canTouch = false;
        }
    } // for exit
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<PlayerController>() != null && canTouch) {
            exitAction?.Invoke();
            canTouch = false;
        }
    } // trigger for exit
}
