using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Cinemachine;
using System;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using Bones;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// This class is going to handle alot. the player player movement. interaction and held item
    /// </summary>
    // Start is called before the first frame update

    #region General Variables

    public static PlayerController Instance;
    // Singleton :)
    private int playerState; // player state 0 = in control, 1 = not. for when in UI.

    #endregion
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } // create singleton instance
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen so it doesnot go everywhere while playing
      
        // -- refrances --
        rb = GetComponent<Rigidbody>(); // grab the refrance
    }
    // Update is called once per frame
    void Update()
    {
        CheckState(); // checks the player state to see if hes in ui
        ButtonInput(); // reads all button input. regaurless of state
        CheckMoveing(); // checkcs to see if we are moving and fires events off
        if (playerState == 0) {
            Movement();
            CameraMovement();
            CheckHoverInteractable();
        } // if the player is in 0/ regular movement let em walk and look around with the cameras
    }// Main Update For the player
    private void ButtonInput() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        } // Jumping
        if (Input.GetKeyDown(KeyCode.Delete)) {
            Application.Quit();
        } // Quit
        if (Input.GetKeyDown(KeyCode.E)) {
            if (playerState != 0)
                return;
            InteractionController();
        } // Interaction
        if (Input.GetKeyDown(KeyCode.R)) {
            ThrowItem();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (playerState == 0) {
                EnterOptionsMenu();
                return;
            } // if we are not in a ui we open the options menu
            if(currentUIPanel == optionsMenu) { // we check to see if the current ui pannel is the options menu, if so we un show it else we deleate the temp UI
                ExitOptionsMenu();  // Exit Options
            } else {
                ExitUIState(); // Exit UI
            }
        }
    }// This Function reads all the input from the player

    #region Camera And Movement

    // Events
    public event Action<float> evt_PlayerMoveing; // when the player is moving, passes in speed of the player
    public event Action evt_PlayerStoped; // for when the player stops moveing
    public event Action evt_PlayerJumping; // for when the player jumps


    //Camera
    [SerializeField] private Camera playerCamera; // the camera 
    public Camera PlayerCamera { get { return playerCamera; } } // getter for the camera variable

    [SerializeField] private Transform playerCamRig;
    private Vector2 mouseDelta;
    [SerializeField] private float cameraSmoothing;
    [SerializeField] private float mouseSensitivity; // Mouse sensitivity for loocking around
    private Vector2 mouseLook; // The value that will be added tot he camera rotation each frame

    //Movement
    private Rigidbody rb; // refrance to the rigidbody attached to the player
    private Vector2 movementDelta;
    private Vector3 moveVector;
    [SerializeField][Range(0.01f, 1)] private float moveSpeed; // your base ground movespeed
    [SerializeField][Range(0, 20f)] private float maxSpeed; // max speed you can go
    [SerializeField][Range(0, 100f)] private float decelerationSpeed; // how fast to accelorate
    [SerializeField] private float sprintSpeedMultipler; // the number your speed is multiplied by when your sprinting
    [SerializeField] private float midAirSpeedMultiplier; // speed applyed while not grouned
    [Space]
    [SerializeField] private float sphereCastStartOffset;
    [SerializeField] private float sphereCastRadias;
    [SerializeField] private float sphereCastMaxDistance;

    //Jumping
    [SerializeField][Range(2, 1000f)] private float jumpAcceleration; // how fast you jump
    private void CameraMovement() {
        mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2.Scale(mouseDelta, new Vector2(mouseSensitivity * cameraSmoothing, mouseSensitivity * cameraSmoothing));
        //here we gather a mouse delta using the x and y axis of the mouse. We then scale that vector using the sensitivity and smoothing.

        Vector2 SmoothCam = Vector2.zero; // A vectot thatw will house the final values we will be adding to the mouse look

        SmoothCam.x = Mathf.Lerp(SmoothCam.x, mouseDelta.x, 1 / cameraSmoothing);
        SmoothCam.y = Mathf.Lerp(SmoothCam.y, mouseDelta.y, 1 / cameraSmoothing);
        // we lerm the values from the mouseDelta to the 1/Smoothing factor

        mouseLook += SmoothCam; // Add the smooth cam to the look vector

        mouseLook.y = Mathf.Clamp(mouseLook.y, -90, 90); // here we clamp the vertical axis to 90 and - 90 so we cant flip the camera around
        
        transform.rotation = Quaternion.AngleAxis(mouseLook.x, transform.up); // rotates the player
        playerCamRig.transform.localRotation = Quaternion.Euler(-mouseLook.y, mouseLook.x, 0); // rotates teh camera on the  vertical and horizontal.
    }
    private void Movement() {
        movementDelta = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // this created a move delta
        moveVector = (transform.forward * movementDelta.y + transform.right * movementDelta.x).normalized; // this the move vector. we normilze it to a value of 1, to avoid speeding up while going diagonally

        float finalSpeed = moveSpeed * GetSpeedMultiplier(); // speed after calculation

        rb.AddForce(moveVector * finalSpeed, ForceMode.VelocityChange); // add force

        ClampVelocity(); // clamp speed

        Decelerate(); // decelerate if we are not moveing
    } // this function handles moveing the player
    private void Decelerate() {
        if (movementDelta != Vector2.zero)
            return;
        rb.velocity = new Vector3(
            Mathf.Lerp(rb.velocity.x, 0, decelerationSpeed),
            rb.velocity.y,
            Mathf.Lerp(rb.velocity.z, 0, decelerationSpeed)
        );
    } // Decelerates the player slowly after not hitting the move keys
    private void ClampVelocity() {
        float maxSpeedWithMultipliers = maxSpeed * GetSpeedMultiplier();
        float velcotyXZMagnitude = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        if (velcotyXZMagnitude < maxSpeedWithMultipliers)
            return;
        Vector2 clampedVelocity = Vector2.ClampMagnitude(new Vector2(rb.velocity.x, rb.velocity.z), maxSpeedWithMultipliers);
        rb.velocity = new Vector3(
          clampedVelocity.x,
          rb.velocity.y,
          clampedVelocity.y
          );
    }// clams our max speed
    public float GetSpeedMultiplier() {
        float speedMultiplier = 1;
        if (IsSprinting())
            speedMultiplier *= sprintSpeedMultipler;
        if (!IsGrounded())
            speedMultiplier *= midAirSpeedMultiplier;
        return speedMultiplier;
    } // this function dictates our speed multiplier by the current player state.
    // --- Jumping ---
    private void Jump() {
        if (!IsGrounded() || playerState != 0)
            return;
        rb.AddForce(Vector3.up * jumpAcceleration, ForceMode.Impulse);
        evt_PlayerJumping?.Invoke();
    } // this sets the jumping state to true and makes us jump
    #endregion

    #region Movement Checks
    // -------------------------- Checks ------------------------------------------- 
    private bool CanSprint() => playerState == 0; // Checks if we can sprint
    private bool IsFalling() => !IsGrounded() && rb.velocity.y < 0; // this function tests if we are falling
    private bool IsWalking() => rb.velocity.z != 0 && rb.velocity.x != 0; // Checks To See If the Player Is Moveing at all
    private bool IsGrounded() { // Here we have the grounded function. this checks weather you are on solid ground;
        bool didHit = false;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position - new Vector3(0, transform.localScale.y - sphereCastStartOffset, 0), sphereCastRadias, Vector3.down, sphereCastMaxDistance);
        foreach (RaycastHit hit in hits) {
            if (hit.collider.gameObject != gameObject) {
                didHit = true;
                break;
            }
        }
        if (!didHit) return false; // if it did NOT hit return false
        return true; // we hit something thats not us
    } // this function checks if the player is grounded
    private bool IsSprinting() {
        if (!CanSprint())
            return false;
        if (Input.GetKey(KeyCode.LeftShift))
            return true;
        return false;
    } // Checks to see if the player is sprinting
    private void CheckMoveing() {
        if (movementDelta != Vector2.zero && IsGrounded() && playerState == 0) {
            evt_PlayerMoveing?.Invoke(moveSpeed * GetSpeedMultiplier());
        } else {
            evt_PlayerStoped?.Invoke();
        }
    } // this function checks if we are moving or not, if so fires the moving event and if not sotps the event
    #endregion

    #region Player State
    private void CheckState() { 
        if(playerState == 0) { // if we are in regular mode, lock the cursour and hide it, else show it and rekease it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // we stop our movement, but not the y so we dont stnad mid air while in UI
        }
    }
    #endregion

    #region Interaction System
    [SerializeField] private float interactionRange; // how far we can interact with stuff from

    // -- Events --
    public event Action<GameObject> evt_OnHover; // for hovering
    public event Action<GameObject> evt_OnInteract; // for interaction
    public event Action<GameObject> evt_OnUse; // for interaction

    // the reason i grab the entire game object here is so i still can reach those other unity properies.
    private void InteractionController() {
        if (heldItem == null) {
            CheckInteraction(); // if we dont have an item we check interaction
        } else {
            if (!CheckUseItem()) { // if we cant use the item. like on a lock or somehting, it puts the item down
                CheckDropItem();
            }
        }
    } // this function controlls what happens when we interact with anything
    private bool CheckHoverInteractable() {
        Collider collider = CheckRayHitInteractable(); ;
        if (collider == null)
            return false;
        IInteractable interactable = collider.GetComponent<IInteractable>();
        if (interactable.CanInteract()) {
            interactable.OnHover(); // calls the on hover on the hovered on object
            evt_OnHover?.Invoke(collider.gameObject);
            return true;
        }
        return false;
    } // chekcs if we are hovering over something interactable
    private bool CheckInteraction() {
        Collider collider = CheckRayHitInteractable();  // gets the collider of what we hit with a ray
        if (collider == null) // if we did not hit anything return false
            return false;
        IInteractable interactable = collider.GetComponent<IInteractable>(); // cast it into an IInteractable
            if (interactable.CanInteract()) { // if we can unteract
                interactable.OnInteract(); // on intereact
                evt_OnInteract?.Invoke(collider.gameObject); //on interact
                return true;
            }
        return false;
    } // checks interaction with anything
    private Collider CheckRayHitInteractable() {
        RaycastHit hit;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(.5f, .5f)); // make a ray from the center of the screen
        bool didHit = Physics.Raycast(ray, out hit, interactionRange); 
        if (didHit) {
            if (hit.collider.GetComponent<IInteractable>() != null) {
                return hit.collider;
            }
        }
        return null;
    } // checks a raycast against the interactable

    // -- Interaction with items -- 
    private void CheckDropItem() {
        RaycastHit hit;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(.5f, .5f));
        bool didHit = Physics.Raycast(ray, out hit, interactionRange); // make aray
        if (didHit) { // if we hit something we put it down at the poit plus the normal of the surface with a little padding, lets us put item on thing cleanly
            float distance = .1f;
            heldItem.Droped(hit.point+(hit.normal*distance)); // drop
        } else {
            heldItem.Droped(playerCamera.ViewportToWorldPoint(new Vector3(.5f, .5f)) + (transform.forward * interactionRange*.5f)); // drop
        } // if we did not hit antyhting puts it out if front
    } // tries to drop the held item and find the rifght location
    private bool CheckUseItem() {
        if (heldItem == null)
            return false; // we need an item in hand to use, so if we dont have an item we reutn
        Collider collider = CheckRayHitInteractable();
        if (collider == null)
            return false; // if we dont hit, reuturn.
        if (collider.GetComponent<Item>() != null)
            return false; // we dont want to be able to pick up 2 items. so if there is an item we return.
        InteractableAction interactable = collider.GetComponent<InteractableAction>(); // get the interactable
        if (interactable == null)
            return false; // if the object we used the item on does not have an interactable actiove return false
        if (interactable.CanInteract()) { // can we ?
            interactable.OnInteract(); // Interact.
            heldItem.Used(); // We used the item. this ques if for destruction
            evt_OnUse?.Invoke(collider.gameObject); 
            return true;
        }
        return false;
    }// sees if we can use an item on a lock or something
  
    #endregion
    
    #region Player UI

    // -- Events --

    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private Transform canvasUI; // the active canvas that the ui will be rendered on
    private GameObject currentUIPanel; // the currently active ui element
    public GameObject CurrentUIPanel { get => currentUIPanel; } // a getter for the current UI element
    public void EnterUIState(GameObject UIPannel, ReadableNoteData data) {
        playerState = 1; // sets the player state to 1 so the player cant move
        currentUIPanel = Instantiate(UIPannel, canvasUI.transform); // instantiates the selected type of ui as a child of the canvas so its rendered 
      
        if(currentUIPanel.GetComponent<ReadableNoteUI>() != null) {
            currentUIPanel.GetComponent<ReadableNoteUI>().SetNoteTextData(data); // sets the data inside to be the actual text
        } // checks to see if it has the readable note companent, and if so pushes the note data in it.
        
    } // Enters the readable note UI state. 
    public void ExitUIState() {
        playerState = 0; // sets that state to 0 so the player can move freely
        if (currentUIPanel != null) {  // if the current ui pannel is not null, this would never get called if the player's current ui was null be we check anyway
            Destroy(currentUIPanel.gameObject); // if its no null destory it
        }
    } // Exits the readble note UI state;

    // for options menu
    public void EnterOptionsMenu() {
        playerState = 1;
        currentUIPanel = optionsMenu;
        optionsMenu.SetActive(true);
    } // opens the options menu

    public void ExitOptionsMenu() {
        playerState = 0;
        currentUIPanel = null;
        optionsMenu.SetActive(false);
    } // exits the options menu
    #endregion

    #region Item Interaction System
    private Item heldItem;
    public Item HeldItem { get => heldItem; set => heldItem = value; }
    [SerializeField] private float throwForce;
    private void ThrowItem() {
        if (heldItem == null)
            return;
        heldItem.Thrown(playerCamera.transform.forward, throwForce);
    } // we throw the item with force forward
    #endregion

    #region Player Death and respawn 
    [SerializeField] Transform respawnPoint;
    public event Action evt_PlayerDie;
    public void KillPlayer(int context) {
        if (heldItem != null) {
            heldItem.Droped(transform.position); // drop any items if we are holding any
        } // drops item if holdint any
        evt_PlayerDie?.Invoke();

        transform.position = respawnPoint.position;
    } // kills the player and respawns him and a point
    #endregion

    #region Game State
    public void ExitGame() {
        Application.Quit();
    } // quits the game 
    #endregion
}
