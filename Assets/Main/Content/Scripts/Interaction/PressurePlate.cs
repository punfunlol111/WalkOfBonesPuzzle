using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    /// <summary>
    /// This class handles the pressurePlate animation
    /// </summary>
    Animator animator; // aniamtor refrance

    private static string PRESSED = "Pressed";
    private static string RELEASED = "Released"; // Refances to the animator perametors

    void Start()
    {
        animator = GetComponent<Animator>();        // grab the animnator
    }
    public void Pressed() {
        animator.SetTrigger(PRESSED);
    } // for when its pressed

    public void Unpressed() { 
        animator.SetTrigger(RELEASED);
    } // for when is released
}
