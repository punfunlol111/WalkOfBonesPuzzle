using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ThrowTarget : MonoBehaviour
{
    /// <summary>
    /// This class is for targets that trigger an event when you throw an item at them
    /// </summary>
    /// 
    public static event Action evt_ThrowTargetBroken;

    public UnityEvent onHitEvent;// triggered when you hit something
    [SerializeField] private bool killOnHit; // destory this when its hit
    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.GetComponent<Item>() != null) {
            Item collidedItem = collision.collider.GetComponent<Item>(); // checking to see if the collision is with an item
            if(collidedItem.GetComponent<Rigidbody>().velocity.magnitude >= 1f) { // here we check that the itemstate is being thrown or 2. 
                onHitEvent?.Invoke();
                // checks to see if what hit it was an item, if so cheks that the speed was high enough to "Break it" if we want to kill on hit, triggers the event and ques the item for destruction
                if (killOnHit) {
                    evt_ThrowTargetBroken?.Invoke();
                    Destroy(gameObject, .2f);
                    Destroy(this);
                }
            }
        }
    }
}
