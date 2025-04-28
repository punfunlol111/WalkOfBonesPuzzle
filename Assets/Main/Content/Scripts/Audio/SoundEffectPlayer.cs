using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    /// <summary>
    /// This script will handle playing sound effects, by creating a tempo object with a listen to play them
    /// </summary>

    private AudioClip clip; // the clip that plays
    private AudioSource source; // the source it plays from

    private float volumeMultiplier; // the volume control
    private float duration;
    void Awake()
    {
        source = GetComponent<AudioSource>(); // get the source
    }
    public void PlayEffect(AudioClip clip, float volumeMult) {

        this.clip = clip;
        volumeMultiplier = volumeMult; // set the internal values to the passed in ojnes

        source.clip = clip; // set the sourcfes clip and volume
        source.volume *= volumeMult;

        duration = clip.length; // set the duration to the duration opf the clip

        source.Play(); // play

        Destroy(gameObject, duration + .5f); // que the item for destruction after its done
    } // sets the internal value of the clip and volume
}
