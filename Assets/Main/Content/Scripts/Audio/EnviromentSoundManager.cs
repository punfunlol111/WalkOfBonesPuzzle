using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSoundManager : MonoBehaviour
{
    /// <summary>
    /// This class plays the sounds on loop in the background
    /// </summary>
    /// 
    private AudioSource source;
    [SerializeField] private AudioClip clip;
    void Start()
    {
        source = GetComponent<AudioSource>(); // get the source
        source.loop = true; // loop it
        source.clip = clip; // set the clip
        Play(); // play
    }
    public void Play() {
        source.Play(); // plays it
    } // starts

    public void Stop() {
        source.Stop(); // if we need to stop
    } // stops it
}
