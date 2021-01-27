using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrinderMinigame : MonoBehaviour{
    #region Public Varirables
    [Header("Rigidbodies and other basic stuff")]
    [Tooltip("This is the transform for the Grinder Selector bit of the grinder")]
    public Transform GrinderSelection;

    [Header("Minigame Objects and Variables")]
    [Tooltip("This is how fast the Griender Selector moves")]
    public float GrinderSelectMoveSpeed = .125f;

    [Tooltip("This adjusts how long a player has to hold to grind")]
    public float GrindTimeLength;
 

    [Tooltip("The particle system for the Coffee Grounds")]
    public ParticleSystem GroundsPS;

    [Header("UI and Stat Manager")]
    [Tooltip("This is the manager for the stats so we can update them")]
    public GameObject StatManager;

    [Tooltip("This is the cooking manager so it manages the minigames and order etc")]
    public GameObject CookingManager;

    [Tooltip("This is the instructions so players know what to fuckin do")]
    public Text Instructions;

    [Tooltip("")]

    #endregion

    #region Private Variables

    #region Grinder Selection Varirables
    /**
        These set of varirables help control the the grinder selector thing
        the lil nub that and also the number value
    **/

    /**
        Grinder Location is just for the selector to have reference on where to move
        and where it starts
    **/
    private float GrinderLocationX;
    private float GrinderLocationY;

    //GrindSize is how fine the grounds are which will affect the texture and the size of the particles
    private int GrindSize = 1;

    //Grind time is a non-destructive way of using the GrindTimeLength so the original time doesn't get yeeted
    private float GrindTime;

    #endregion

    #region Booleans
    /**
        Simple set of booleans that help control if things are being used/active/etc
    **/

    //This sets it so you cannot change the grind after you start
    private bool CanChangeGrind = true;
    //This checks so you don't add the value multiple times
    public bool ValueAdded = false;
    //This is so that you can't play the game if it's not active
    private bool MinigameActive = false;

    //This is so you can signal to the Cooking Controller that the minigame is complete
    private bool MinigameCompleted = false;

    #endregion

    #endregion


    // Start is called before the first frame update
    void Start(){
        //Sets the grinder locations to a variable
        GrinderLocationX = GrinderSelection.position.x;
        GrinderLocationY = GrinderSelection.position.y;
        //Sets GrindTime to GrindTimeLength for non-destructive use
        GrindTime = GrindTimeLength;
    }

    // Update is called once per frame
    void Update(){
        //Only plays if the minigame is active and running
        if(MinigameActive)
            RunMinigame();
    }

    #region Minigame Controls
    /**
        As always there's this one cohesive function to run so that way life is easier
    **/
    private void RunMinigame(){
        //If the minigame is NOT completed
        if(!MinigameCompleted){
            if(CanChangeGrind){
                AdjustSelection();
                MoveGrindSelection();
            }
            GrindBeans();
            UpdateInstructions();
        }

        //If the minigame is completed
        if(MinigameCompleted){
            if(GroundsPS.isPlaying)
                GroundsPS.Stop();
            CookingManager.GetComponent<CookingController>().MinigameFinished();
        }

    }

    #region Minigame Functions

    /**
        This allows the player to select the grind size number, moving left and right
        will shift the grind size up or down by 1
    **/
    private void AdjustSelection(){
        /**
            Does a simple check left/right to see and if it too big or small dont change
        **/
        if(Input.GetButtonDown("Right")){
            if(GrindSize < 10)
                GrindSize++;
        } else if (Input.GetButtonDown("Left")){
            if(GrindSize > 1)
                GrindSize--;
        }

    }

    /**
        This VISUALLY moves the grind selection in the game. It does not actually
        affect the variable whatsoever.It does this by just changing it'sposition very slowly
        using the GrinderSelectMoveSpeed Variable
    **/
    private void MoveGrindSelection(){
        float newXPos = GrinderLocationX + (0.6f * (GrindSize - 1));
        Vector3 NewLocation = new Vector3(newXPos, GrinderLocationY, 0);

        Vector3 MovementVector = new Vector3(GrinderSelectMoveSpeed, 0, 0);

        if(GrinderSelection.position.x < NewLocation.x)
            GrinderSelection.position = GrinderSelection.position + MovementVector;

        if(GrinderSelection.position.x > NewLocation.x)
            GrinderSelection.position = GrinderSelection.position - MovementVector;

        GroundsPS.startSize = 0.1f+ GrindSize * 0.05f;
    }

    /**
        This grinds the beans and adds the stats to the texture
        It also forces the player to hold a button for like 2 seconds
        isn't that fun. It's okay you're rewarded with particles!
    **/
    bool ShowGrounds = false;
    private void GrindBeans(){
        StatManager.GetComponent<FoodStats>().UpdateTexturePreview(GrindSize * 10);

        if(Input.GetButtonDown("Use") && !ValueAdded){
            StatManager.GetComponent<FoodStats>().AddTexture(GrindSize * 10);
            ValueAdded = true;
            CanChangeGrind = false;
        }

        if(Input.GetButton("Use")){
            ShowGrounds = true;
            if(GrindTime > 0){
                GrindTime -= Time.deltaTime;
            } else {
               MinigameCompleted = true;
            }

        } else if(Input.GetButtonUp("Use")){
            ShowGrounds = false;
        }

        if(ShowGrounds){
            if(!GroundsPS.isPlaying)
                GroundsPS.Play();
        } else {
            if(GroundsPS.isPlaying)
                GroundsPS.Stop();
        }

    }


    /**
        This updates the instructions text so it properly displays what to do and stuff
    **/
    private void UpdateInstructions(){
        if(MinigameCompleted == false){
            Instructions.text = "Press left and right to adjust the grind size.\n Hold Space to grind!";
        } else {
           Instructions.text = "You did it binch! Click Next to move onto the next step!";
        }
    }
    #endregion
    #endregion

    #region Cooking Controller Actions
    //ActivateMinigame gets called to activate the minigame so it can actually run
    public void ActivateMinigame(){
        MinigameActive = true;
    }

    //DeactivateMinigame turns the minigame off so you can't play it
    public void DeactivateMinigame(){
        MinigameActive = false;
    }
    #endregion

}
