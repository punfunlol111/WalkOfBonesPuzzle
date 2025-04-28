using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SpinAndMoveBottles : MonoBehaviour
{
    /// <summary>
    /// This is a calss that handles the spining moveing bottes inthe title screen
    /// </summary>
    public float rotationSpeed; // how fast they spon
    [SerializeField] private Transform rotationPoint; // the point they rotate around
    public float timeTillHoverOver; // time till the hover is over
    void Start()
    {
    }
    void Update()
    {
        if (timeTillHoverOver>0) {
            transform.RotateAround(rotationPoint.position, transform.forward, rotationSpeed*2);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.25f, 1.25f, 1.25f),.1f);
        } else {
            transform.RotateAround(rotationPoint.position, transform.forward, rotationSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, .1f);
        }

        if (timeTillHoverOver > 0) {
            timeTillHoverOver -= 1*Time.deltaTime;
        }
        // we give a little padding time so it still rotates for a sec after we are not hovering over it
        // it gives a more smooth vibe
    }
}
