using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Bones;
public class ReadableNoteUI : MonoBehaviour
{
    /// <summary>
    /// This will be attached to the actual ui inside the canvas and refrance the components like the tital and the paragraph, we do this here serialized ourselves so we dont 
    /// need to use FindObject.
    /// </summary>

    // make all this private so you cant change what they refrance
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text paragraph;
    [SerializeField] private TMP_Text hint;
    [SerializeField] private TMP_Text answer; 

    // all these are only getters to the data
    public TMP_Text Title { get { return title; } }
    public TMP_Text Paragraph { get { return paragraph; } }
    public TMP_Text Hint { get { return hint; } }
    public TMP_Text Answer { get { return answer; } }

    private void Start() {
        hint.gameObject.SetActive(false); // set the hint to be hidden on start
        answer.gameObject.SetActive(false); // set the answer text to hide on start
    }

    public void ActivateHint() {
        hint.gameObject.SetActive(true);
    } // when the hint button is pressed activate the hint/ un hide it

    public void ActivateAnswer() {
        answer.gameObject.SetActive(true);
    } // when the answer button is pressed activates the answer text

    public void SetNoteTextData(ReadableNoteData data) {
        title.text = data.title;
        paragraph.text = data.paragraph;
        hint.text = data.hint;
        answer.text = data.answer;
    } // sets the actual text in the ui with the data passed in
}

namespace Bones { // this is a struct that will hold all the not data we need
    public struct ReadableNoteData {
        public string title; // title of the note
        public string paragraph; // main text / clue
        public string hint; // a hint if its to hard
        public string answer; // the answer if you cant figure it out

        public ReadableNoteData(string _title, string _paragraph, string _hint, string _answer) {
            title = _title;
            paragraph = _paragraph;
            hint = _hint;
            answer = _answer;
        }
    }
}
