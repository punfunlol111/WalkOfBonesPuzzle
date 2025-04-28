using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SawBlade : MonoBehaviour {

    #region Refrances
    private TouchKill touchKill; // a refrance to the touch kill script attached
    private Rigidbody rb; // refrance to the rigidbody
    private Collider col; // refrances the collider on the blade
    #endregion
    

    [SerializeField] private bool startActive; // do we want the sawblade to start Active
    private bool isActive; // is the sawblade currently active

    [SerializeField] private bool isStatic; // is the sawblade currently static or mobile ?
 
    [SerializeField] Transform[] targetPoints; // the list of points the sawblade will travel to if mobile
    private int currentTargetPointIndex = 0; // the current target of the sawblade, in index form

    [SerializeField] private float moveSpeed; // how fast the saw blade moves
    [SerializeField] private float rotationSpeed; // how fast the sawblade rotates

    private void Awake() {
        touchKill = GetComponent<TouchKill>(); // gets the toch kill sctript
        rb = GetComponent<Rigidbody>(); // gets the rigidbody
        col = GetComponent<Collider>();
    }
    void Start() {
        if(targetPoints.Length < 2) 
            isStatic = true; // if we have less then two point then we make the blade static. there is no where to move to

        if (startActive) // if we want to start with the sawblade active we do. and if not, then not
            ActivateSawBlade();
        else
            DeactivateSawBlade();
    }

    void Update() {
        if (!isActive) { 
            
            return; // if we are not active dont do any game logic
        }
        RotateSawBlade(); // rotate the sawblade so it spins
        if (isStatic) { // if this saw Blade is static
            rb.velocity = Vector3.zero;
        } else { // if the saw blade is moveing
            MoveToPoint();    
        } 
    }

    #region Movement
    private void RotateSawBlade() {
        rb.AddTorque(new Vector3(0, 0, rotationSpeed));
    } // adds a rotation to the sawblade.
    private void MoveToPoint() {
        rb.AddForce((targetPoints[currentTargetPointIndex].position - transform.position).normalized * moveSpeed, ForceMode.VelocityChange);
        if (Vector3.Distance(transform.position, targetPoints[currentTargetPointIndex].position) < .2f) {
            ChangeDirection();
        } // changing direction
    } // here we move the sawblade to the current point and check if we are close to it, if we are we change direction to the next point
    private void ChangeDirection() {
        rb.velocity = Vector3.zero;
        currentTargetPointIndex++;
        if (currentTargetPointIndex >= targetPoints.Length) {
            currentTargetPointIndex = 0;
        }
    } // changes the direction of the sawblade to the next point in line and loops back to the first one if we go past the number of points
    #endregion

    #region Saw blade state;
    public void ActivateSawBlade() {
        isActive = true;
        touchKill.Activate();
        rb.freezeRotation = false;
        col.isTrigger = true;
        rb.isKinematic = false;
    } // activates the sawblade

    public void DeactivateSawBlade() {
        isActive = false;
        touchKill.Deactivate();
        rb.velocity = Vector3.zero;
        rb.freezeRotation = true;
        col.isTrigger = false;
        rb.isKinematic = true;
    } //deactivates the sawblade so its harmless

    public void FlipSawbladeState() {
        if (isActive)
            DeactivateSawBlade();
        else
            ActivateSawBlade();
    } // if the saw blade is active, deactivates it and vice versa

    public void SetSawbladeStatic() => isStatic = true;

    public void SetSawbladeMobile() => isStatic = false;

    #endregion
}
