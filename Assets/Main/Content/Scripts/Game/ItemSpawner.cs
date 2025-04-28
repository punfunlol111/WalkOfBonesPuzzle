using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpaner : MonoBehaviour
{
    /// <summary>
    /// This script will sapwn an item and if it gets to far from the origin will spawn another one
    /// </summary>
    /// 
    [SerializeField] private GameObject objectToSpawn; // the object that we spawn
    private GameObject currentObject; // the current refrance object
    [SerializeField] private Transform spawnPoint; // the positin at which we spawn the item
    [SerializeField] private float spawnRange; // how far away the item needs to be to spawn a new one
    void Update()
    {
        CheckSpawn();
    }

    private void CheckSpawn() {
        if (currentObject == null) { 
            SpawnItem();
            return; // we return here not to double spawn and refrance null
        } // if there is no current item. we set it. 

        if (Vector3.Distance(spawnPoint.position, currentObject.transform.position) > spawnRange) {
            SpawnItem();
        }
    } // shuld  we spawn an item

    private void SpawnItem() {
        GameObject obj = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity, transform);
        currentObject = obj; 
    } // spawns the object and sets the current one to the one that was just spawned
}
