using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TouchKill : MonoBehaviour
{
    /// <summary>
    /// This is a class for stuff that kills the player. basicly just a death obsticle
    /// </summary>
    private bool active; // if the killing touch is active. does nothing if not active

    private void Start() {
        active = true;
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.GetComponent<PlayerController>() != null && active) {
            collision.collider.GetComponent<PlayerController>().KillPlayer(0);
        }
    } // if the collider is Not a trigger

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>() != null && active) {
            other.GetComponent<PlayerController>().KillPlayer(0);
        }
    } // if the collision is a trigger 

    public void Activate() => active = true; //activates 
    public void Deactivate() => active = false; // Deactivates
    public bool IsActive() => active;    // checks if active
    public void FlipState() {
        if (active)
            active = false;
        else
            active = true;
    } // if we are active deactivate, if we are not active, activate
}
