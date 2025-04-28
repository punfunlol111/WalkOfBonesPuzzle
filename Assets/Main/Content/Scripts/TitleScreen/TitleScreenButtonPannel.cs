using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleScreenButtonPannel : MonoBehaviour
{
    [SerializeField] private bool quitGame; // id true the button will quite the game
    [SerializeField] private int titleID; // what scene we get taken to
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // makes sure the curser can move
    }

    void Update()
    {
        HoverRay(); // checks if we are hovering over the bottle for the faster spin effect
        if (Input.GetMouseButtonDown(0)) { // checks to see if we clicked the button
            ShotClickRay(); // does the check
        }

    }
    private void ShotClickRay() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f)){
            if (hit.collider.GetComponent<TitleScreenButtonPannel>() != null) {
                TitleScreenButtonPannel button = hit.collider.GetComponent<TitleScreenButtonPannel>();
                button.Action();
            }
        } // raycast checkinbg  for title screen button if so calling the action function
    }
    private void HoverRay() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f)) {
            if (hit.collider.GetComponent<SpinAndMoveBottles>() != null) {
                SpinAndMoveBottles bottle = hit.collider.GetComponent<SpinAndMoveBottles>();
                bottle.timeTillHoverOver = .2f;
            }
        }
    } // checks to see if we are hovering over an action button
    public void Action() {
        if (quitGame) { // if the button is set to quit the game it quits the game
            QuitGame();// quits the ganme
            return; // so we dont also start the game
        }
        StartGame(titleID); // loads the scene with the titleID.
    } // when the button is pressed this is called
    public void QuitGame() {
        Application.Quit();
    }

    public void StartGame(int buildIndex) {
        SceneManager.LoadScene(buildIndex);
    } // loads the scenece with the requested build index
}
