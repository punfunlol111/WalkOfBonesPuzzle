using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Bones;

public class NoteSystemUI : MonoBehaviour, IInteractable
{

    public UnityEvent onInteractEvent; // this is optianal if you want an event to fire when reading sokmething

    [SerializeField] private GameObject noteUI; // the ui thats pulled up when reading

    [SerializeField] private string title; // title if there is 
    [SerializeField] private string paragraph; // the text
    [SerializeField] private string hint; // the hint 
    [SerializeField] private string answer; // the answer
    private ReadableNoteData noteData; // holds all the note data in one small package :)
    public void OnInteract() {
        SetNoteData(); // sets the note data

        PlayerController.Instance.EnterUIState(noteUI, noteData); // calls the the creation of the ui inside the canvas, and passes over out not data to be set iunto the UI
     
        onInteractEvent?.Invoke(); // invoke the action of interation, incase we want extra action
    }
    private void SetNoteData() {
        noteData = new ReadableNoteData(
            title,
            paragraph,
            hint,
            answer
        );
    } // this function populates the note data
}
