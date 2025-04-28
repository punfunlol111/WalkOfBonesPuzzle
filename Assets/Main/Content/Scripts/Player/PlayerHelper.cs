using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    /// This Script is to make function that will be called by unity events. so i dont havbe to make alot of scripts///
    public void TeleportPlayer(Transform newPosition) => transform.position = newPosition.position;
}
