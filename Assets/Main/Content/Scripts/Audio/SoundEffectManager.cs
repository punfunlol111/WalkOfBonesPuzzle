using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    /// <summary>
    /// This is gonna be the class that handles spawjnining in sound effects. it will be singlton so there is only one instance of it at all times;
    /// This is also gonna listen to all the classes and play sound effects when needed.
    /// </summary>
    /// 
    public static SoundEffectManager Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this; // singleton pattern
    }

    private void Start() {
        Subscribe(); // subscribe to all the events
    }

    #region Playing Sound
    [SerializeField] private GameObject soundEffect; // the sound effect game object
    public void PlaySound(AudioClip clip, float volumeMult, Vector3 where) {
        GameObject spawnedEffect = Instantiate(soundEffect, where, Quaternion.identity, null);
        // instatiate a soundEffect object at a place and make the parent this object
        spawnedEffect.GetComponent<SoundEffectPlayer>().PlayEffect(clip, volumeMult);
    }// plays a sound at a position

    public void PlaySoundSimple(AudioClip clip) {
        GameObject spawnedEffect = Instantiate(soundEffect, PlayerController.Instance.transform.position, Quaternion.identity, gameObject.transform);
        // instatiate a soundEffect object at a place and make the parent this object
        spawnedEffect.GetComponent<SoundEffectPlayer>().PlayEffect(clip, 1);
    } // plays a sound, good for unity events thatc an only use one paramator
    #endregion

    #region Event Listening
    private void Subscribe() {
        Item.evt_OnCollideAfterBeingThrown += Item_evt_OnCollideAfterBeingThrown;
        Item.evt_OnPickUp += Item_evt_OnPickUp;
        Item.evt_OnPutDown += Item_evt_OnPutDown;
        SingleSmallDoor.evt_DoorOpen += SingleSmallDoor_evt_DoorOpen;
        PlayerController.Instance.evt_PlayerJumping += PlayerController_evt_PlayerJumping;
        PlayerController.Instance.evt_PlayerDie += PlayerController_evt_PlayerDie;
        SingleSmallDoor.evt_DoorClose += SingleSmallDoor_evt_DoorClose;
        DartTrap.evt_DartTrapFired += DartTrap_evt_DartTrapFired;
        Button.evt_ButtonPress += Button_evt_ButtonPress;
        NoteSystemUI.evt_OnInteractWithNote += NoteSystemUI_evt_OnInteractWithNote;
    }// this function subscribes to all the events that play sound

    private void NoteSystemUI_evt_OnInteractWithNote() {
        PlaySound(GetSoundEffect(9), 1.7f, PlayerController.Instance.transform.position);
    } // interacted with note
    private void Button_evt_ButtonPress() {
        PlaySound(GetSoundEffect(7), 1, PlayerController.Instance.transform.position);
    } // button pressed
    private void DartTrap_evt_DartTrapFired() {
        PlaySound(GetSoundEffect(7), 1, PlayerController.Instance.transform.position);
    } // dart trap fires sound
    private void SingleSmallDoor_evt_DoorClose() {
        PlaySound(GetSoundEffect(6), 1, PlayerController.Instance.transform.position);
    } // small door close
    private void SingleSmallDoor_evt_DoorOpen() {
        PlaySound(GetSoundEffect(5), 1, PlayerController.Instance.transform.position);
    } // small door open
    private void Item_evt_OnPutDown(GameObject obj) {
        PlaySound(GetSoundEffect(4), 1, PlayerController.Instance.transform.position);
    } // player puts item down
    private void Item_evt_OnPickUp(GameObject obj) {
        PlaySound(GetSoundEffect(3), 1, PlayerController.Instance.transform.position);
    } // player pick up item
    private void PlayerController_evt_PlayerJumping() {
        PlaySound(GetSoundEffect(2), 1, PlayerController.Instance.transform.position);
    } // player jumps
    private void PlayerController_evt_PlayerDie() {
        PlaySound(GetSoundEffect(1), 1, PlayerController.Instance.transform.position);
    } // player dies
    private void Item_evt_OnCollideAfterBeingThrown(GameObject item) {
        PlaySound(GetSoundEffect(ITEM_THUD_SFX), 1, item.transform.position);
    } // item collides after being thrown
    #endregion

    #region Sound Effect Variables
    [SerializeField] private List<AudioClip> SoundEffects;
    public const int ITEM_THUD_SFX = 0;
    public const int PLAYER_DIE_SFX = 1;
    public const int PLAYER_JUMP_SFX = 2;
    public const int ITEM_PCIKUP_SFX = 3;
    public const int ITEM_PUTDOWN_SFX = 4;
    public const int DOOR_OPEN_SFX = 5;
    public const int DOOR_CLOSE_SFX = 6;
    public const int THROW_TARGET_BROKEN_SFX = 7;
    public const int CLICK_SFX = 8;
    public const int NOTE_PAGEOPEN_SFX = 9;
    public AudioClip GetSoundEffect(int index) {
        if (index > SoundEffects.Count) { 
            return SoundEffects[0];
        } // check to make sure the index is not bigger then the legnth
        return SoundEffects[index];
    } // this gets us the audio clip linked to the index if the sound effects list. 
    #endregion

}
