using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrinderMinigame : MonoBehaviour{
    #region Public Varirables
    [Header("Rigidbodies and other basic stuff")]
    [Tooltip("How much you will add to the texture")]
    public float TextureAdd;
    [Tooltip("This is the transform for the Grinder Selector bit of the grinder")]
    public Transform GrinderSelection;

    [Tooltip("The SFX for the Grinder")]
    public AudioSource GrinderSFX;

    [Tooltip("SFX for the Grinder Clicking")]
    public AudioSource SelectionSFX;

    //#####################################################################################

    [Header("Minigame Objects and Variables")]
    [Tooltip("These are the locations it will go to")]
    public Transform[] SelectionLocations = new Transform[10];
    [Tooltip("This is how fast the Grinder Selector moves")]
    public float GrinderSelectMoveSpeed = .125f;

    [Tooltip("This adjusts how long a player has to hold to grind")]
    public float GrindTimeLength;

    [Tooltip("The particle system for the Coffee Grounds")]
    public ParticleSystem GroundsPS;

    [Tooltip("This is for when the button is pressed")]
    public GameObject ButtonPressed;
    [Tooltip("This is for when the button is NOT pressed")]
    public GameObject ButtonUnpressed;

    [Tooltip("This is the coffee beans in the grinder with 3 states empty, half, and full")]
    public GameObject[] CoffeeFill = new GameObject[3];

    //#####################################################################################

    [Header("UI and Stat Manager")]
    [Tooltip("This is the manager for the stats so we can update them")]
    public GameObject StatManager;

    [Tooltip("This is the cooking manager so it manages the minigames and order etc")]
    public GameObject CookingManager;

    [Tooltip("This is the instructions so players know what to fuckin do")]
    public Text Instructions;

    [Tooltip("This is the canvas used to play the game")]
    public GameObject MinigameCanvas;

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
    private bool ValueAdded = false;
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
        if(MinigameActive){
            RunMinigame();
        } else {
            MinigameCanvas.SetActive(false);
        }
    }

    #region Minigame Controls
    /**
        As always there's this one cohesive function to run so that way life is easier
    **/
    private void RunMinigame(){
        MinigameCanvas.SetActive(true);
        //If the minigame is NOT completed
        if(!MinigameCompleted){
            StatManager.GetComponent<FoodStats>().HidePlus(0);
            StatManager.GetComponent<FoodStats>().HidePlus(1);
            StatManager.GetComponent<FoodStats>().HidePlus(2);

            StatManager.GetComponent<FoodStats>().UpdateTexturePreview(0);
            StatManager.GetComponent<FoodStats>().UpdateWarmthPreview(0);
            StatManager.GetComponent<FoodStats>().UpdateFlavorPreview(0);

            if(CanChangeGrind){
                AdjustSelection();
                MoveGrindSelection();
            }
            GrindBeans();
            UpdateInstructions();  
        }

        //If the minigame is completed
        if(MinigameCompleted){
            MinigameCanvas.SetActive(false);
            ButtonPressed.SetActive(false);
            ButtonUnpressed.SetActive(true);
            StatManager.GetComponent<FoodStats>().HidePlus(0);
            GrinderSFX.Stop();
            if(GroundsPS.isPlaying)
                GroundsPS.Stop();
            StatManager.GetComponent<FoodStats>().UpdateTexturePreview(0);
            CookingManager.GetComponent<CookingController>().MinigameFinished(0, false);
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
            AdjustRight();
        } else if (Input.GetButtonDown("Left")){
            AdjustLeft();
        }

    }

    public void AdjustRight(){
        if(GrindSize < 10 && CanChangeGrind){
            SelectionSFX.Play();
            GrindSize++;
        }
    }

    public void AdjustLeft(){
        if(GrindSize > 1 && CanChangeGrind){
                SelectionSFX.Play();
                GrindSize--;
            }
    }

    /**
        This VISUALLY moves the grind selection in the game. It does not actually
        affect the variable whatsoever.It does this by just changing it'sposition very slowly
        using the GrinderSelectMoveSpeed Variable and lerping it
    **/
    private void MoveGrindSelection(){
        Vector3 NewLocation = SelectionLocations[GrindSize - 1].position;

        Vector3 MovementVector = Vector3.Lerp(GrinderSelection.position, 
                                            NewLocation, 
                                            GrinderSelectMoveSpeed);

        GrinderSelection.position = MovementVector;

        GroundsPS.startSize = 0.1f+ GrindSize * 0.05f;
    }

    /**
        This grinds the beans and adds the stats to the texture
        It also forces the player to hold a button for like 2 seconds
        isn't that fun. It's okay you're rewarded with particles!
    **/
    bool ShowGrounds = false;
    bool IsGrinding = false;
    private void GrindBeans(){
        //StatManager.GetComponent<FoodStats>().UpdateTexturePreview(
        //                                        StatManager.GetComponent<FoodStats>().TextureVal
        //                                         + GrindSize * TextureAdd);

        if(Input.GetButton("Use")){
            IsGrinding = true;
        } else if(Input.GetButtonUp("Use")){
            IsGrinding = false;
        }

        if(IsGrinding && !ValueAdded){
           //StatManager.GetComponent<FoodStats>().AddTexture(GrindSize * TextureAdd);
            ValueAdded = true;
            CanChangeGrind = false;
        }

        if(IsGrinding){
            ButtonPressed.SetActive(true);
            ButtonUnpressed.SetActive(false);
            GrinderSFX.mute = false;
            ShowGrounds = true;
            if(GrindTime > 0){
                GrindTime -= Time.deltaTime;
            } else {
               MinigameCompleted = true;
            }

        } else {
            ButtonPressed.SetActive(false);
            ButtonUnpressed.SetActive(true);
            GrinderSFX.mute = true;
            ShowGrounds = false;
        }

        if(ShowGrounds){
            if(!GroundsPS.isPlaying)
                GroundsPS.Play();
        } else {
            if(GroundsPS.isPlaying)
                GroundsPS.Stop();
        }

        ShowBeans();

    }

    public void ToggleGrinder(){
        IsGrinding = !IsGrinding;
    }

    /**
        This function shows the beans visually so that
        way the player is getting feedback about making them
        beans grounds
    **/
    private void ShowBeans(){
        if(GrindTime <= GrindTimeLength/2 && GrindTime > 0){
            CoffeeFill[0].SetActive(false);
            CoffeeFill[1].SetActive(true);
        } else if(GrindTime <= 0){
            CoffeeFill[1].SetActive(false);
            CoffeeFill[2].SetActive(true);
        }
    }


    /**
        This updates the instructions text so it properly displays what to do and stuff
    **/
    private void UpdateInstructions(){
        if(MinigameCompleted == false){
            Instructions.text = "Click Left and Right to adjust the grind size.\nPress the button on top to grind!";
        } else {
           Instructions.text = "You did it! Click Back to move onto the next step!";
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
