using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrap : MonoBehaviour
{
    /// <summary>
    /// This is a class that will spawn a dart, and shoot it out, once its far away iot will recall it and fire it again
    /// </summary>
    /// 

    public static event Action evt_DartTrapFired;

    [SerializeField] private GameObject projectile; // the projectile that gets fired
    [SerializeField] private Transform spawnPoint;//the point where the projectile is spawned
    [SerializeField] private float recallDistance = 5f; // how far the projectile needs to be from the body till it gets recalled
    [SerializeField] private float shootForce = 10f; // the fire rate;

    void Start()
    {
        SpawnProjectile(); // spawn the projectile at the start
    }

    void Update()
    {
        CheckForRecall(); // checks every update the distance to see if it need sto come back
    }

    private void SpawnProjectile() {
        projectile = Instantiate(projectile, spawnPoint.position, Quaternion.Euler(90, 0, 0), gameObject.transform);
        projectile.SetActive(false);
    } // the inital spawn of the projectile

    public void FireProjectile() {
        projectile.SetActive(true); // sets active
        Rigidbody rb = projectile.GetComponent<Rigidbody>(); //getsd the rigidbody
        rb.velocity = Vector3.zero;
        rb.Move(spawnPoint.position, Quaternion.Euler(90,0,0)); // movbes it to the muzzle
        rb.AddForce(transform.forward * shootForce, ForceMode.Impulse); // shoots it
        evt_DartTrapFired?.Invoke();
    } // fires the projectile

    private void RecallProjectile() {
        projectile.transform.position = spawnPoint.position;
        projectile.SetActive(false);
    } // sends the prjectile back.

    private void CheckForRecall() {
        if(Vector3.Distance(transform.position, projectile.transform.position) > recallDistance) 
            RecallProjectile();
    } // checks to see if the projectile is to far and needs to come back
}
