using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransportNextLevel : MonoBehaviour
{
    public void Transport(int buildIndex) {
        SceneManager.LoadScene(buildIndex);
    } // transports the player to a scence with a spacific build index
}
