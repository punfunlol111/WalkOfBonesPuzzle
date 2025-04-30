using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    /// <summary>
    /// This script is for the all the players
    /// sounds that are not simple and cant use the sound effect player.
    /// </summary>
    /// 
    [SerializeField] private AudioSource walkingSource; // the audio source for walking
    [SerializeField] private float targetWalkVolume; // how loud the footsteps shuld be
    [SerializeField] private AudioClip walkingClip; // the clip that contains the sound of the player walking
    void Start()
    {
        Subscribe(); // subscribe
        walkingSource.clip = walkingClip;
    }

    private void Subscribe() {
        PlayerController.Instance.evt_PlayerMoveing += PlayerController_evt_PlayerMoveing;
        PlayerController.Instance.evt_PlayerStoped += PlayerController_evt_PlayerStoped;
    } // this function subscribes to all the events we need to listen out for
    #region Movement
    private void PlayerController_evt_PlayerStoped() {
        if (walkingSource == null)
            return;
        if (walkingSource.isPlaying) { // are we playing
            walkingSource.volume = Mathf.Lerp(walkingSource.volume, 0, .05f); // if so lerp the volume to zero
            if (walkingSource.volume < .1f) {//if the volume is close to zero stop playing the clip
                walkingSource.Stop(); // stops the clip
            }
        }
    } // called when the player stoped moving
    private void PlayerController_evt_PlayerMoveing(float velocity) {
        if (walkingSource == null)
            return;
        walkingSource.volume = targetWalkVolume; // set the source back to max volume
        walkingSource.loop = true; // loop the clip
        if (!walkingSource.isPlaying) { // if we are not playing start to play
            walkingSource.Play();
        }
        walkingSource.pitch = 1 + (Mathf.Clamp(velocity, 0, 2)); // change the speed based on if we are springting or walking
    } // called while the player is moveing
    #endregion

}
