using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    /// <summary>
    /// This is the spear trap it bounces up and down trying to stab the player
    /// </summary>


    [SerializeField] private Transform startPoint; // the place where the trap starts
    [Range(0.001f, 0.05f)] [SerializeField] private float speed; // how fast the trap moves
    [SerializeField] private float spearRange; // how far down it goes
    private float traveledDistance = 0; // the  distance its traveled
    private Vector3 oldPosition; // the old position

    private bool attacking = true; // are we going down or up
    private bool active = true; // active or now
    void Start()
    {
        transform.position = startPoint.position; // set the pos to the start pos
        oldPosition = transform.position; // set the old postion to the current for nwo
    }

    void Update()
    {
        if (active) // if active and attacking attack else retract
        {
            if (attacking) {
                Attack();
            } else {
                Retract();
            }
        } else {
            transform.position = startPoint.position; // if we are not attacking just stay not active in the up position
        }
    }

    private void Attack() {
        transform.position = Vector3.Lerp(transform.position, (transform.position + Vector3.down*spearRange),speed);
        // move the sepat with lers
        traveledDistance += Vector3.Distance(transform.position, oldPosition);
        // add the distance traveled from the old pos to the current 
        oldPosition = transform.position; // set the old pos to the current
        CheckForDirectionChange(); // are we done?
    } // changes the spears down

    private void Retract() {
        transform.position = Vector3.Lerp(transform.position, startPoint.position, speed);
        CheckForDirectionChange();
    } // lerps back up and checks for direction change

    private void CheckForDirectionChange() {
        if (attacking) { // if sttacking
            if(traveledDistance >= spearRange) { // if the trasvel distance is bigger or equal to the spear range we change dir
                attacking = false; // not attacking
                traveledDistance = 0f; // dist to zero 
            }
        } else {
            if(Vector3.Distance(transform.position, startPoint.position) < .1f) { // if the distance to the start position is small the go back to attacking
                attacking = true; // yes attack
                traveledDistance = 0f; // travled distance 0 (even though its alread zero)
                oldPosition = transform.position; // set the old pos
            }
        }
    } // checks to see if the spears need to change direction

    public void Activate() => active = true;
    public void Deactivate() => active = false;


}
