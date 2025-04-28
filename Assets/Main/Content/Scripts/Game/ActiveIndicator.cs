using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveIndicator : MonoBehaviour
{
    /// <summary>
    /// This is a script for an object that changes color based on if its active opr not. a way to respesent tot he player that hes on the right path
    /// </summary>
    /// 
    [SerializeField] private Material activeMaterial; // green for active
    [SerializeField] private Material inactiveMaterial; // red for inactive
    public void Activeate() {
        GetComponent<MeshRenderer>().material = activeMaterial;
    } // sets the material to green

    public void Deactivate() {
        GetComponent<MeshRenderer>().material = inactiveMaterial;
    } // sets the material to red

    private void Start() {
        GetComponent<MeshRenderer>().material = inactiveMaterial;
    } // on start make sure its red for inactive
}
