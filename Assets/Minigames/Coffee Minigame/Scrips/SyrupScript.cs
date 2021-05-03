using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyrupScript : MonoBehaviour{
  #region Public Variables

    [Header("Minigame Objects and Variables")]
    [Tooltip("The amount you will add to Flavor")]
    //Values forr when you add the flavors
    public int[] FlavorValues = {10, 8, 5};

    [Tooltip("The amount you will add to Texture")]
    public int[] TextureValues = {0, 2, 5};

    [Tooltip("Amount of Pumps")]
    public int PumpsLeft = 5;


    //[Tooltip("Syrup SFX")]
    //public AudioSource SyrupSFX;
   // public Animator Animation;

    [Tooltip("The locations of each syrup bottle")]
    public Transform[] SyrupLocations = new Transform[3];

    [Tooltip("The cup that moves")]
    public Transform Cup;

    [Header("UI and Stat Manager")]
    [Tooltip("This is the manager for the stats so we can update them")]
    public GameObject StatManager;

    [Tooltip("This is the cooking manager so it manages the minigames and order etc")]
    public GameObject CookingManager;

    [Tooltip("This is the instructions so players know what to fuckin do")]
    public Text Instructions;

    [Tooltip("This is the minigame Canvas")]
    public GameObject MinigameCanvas;

    #endregion

    #region Private Variables
    //States for the syrup
    //0 - Vanilla
    //1 - Caramel
    //2 - Chocolate
    private int SyrupState = 0;
    
    //This is so that you can't play the game if it's not active
    private bool MinigameActive = false;

    #endregion


    void Start() {
    }

    // Update is called once per frame
    void Update(){
        if(MinigameActive){
            MinigameCanvas.SetActive(true);
            RunMinigame();
        } else {
            MinigameCanvas.SetActive(false);
            SyrupState = 0;
        }
    }

    #region Minigame Function
    /**
        Since this minigame is so small I shoved everything into one function.
        It just adds the amount from AddAmount to the thing. That's it.
    **/
    private void RunMinigame(){
        ShowPluses();
        MoveCup();

        if(PumpsLeft > 0){
            Instructions.text = "Press left and right to select a syrup and pump to pump!\n" + "Pumps Left: " + PumpsLeft;
            StatManager.GetComponent<FoodStats>().UpdateFlavorPreview(StatManager.GetComponent<FoodStats>().FlavorVal + FlavorValues[SyrupState]);
            StatManager.GetComponent<FoodStats>().UpdateWarmthPreview(0);
            StatManager.GetComponent<FoodStats>().UpdateTexturePreview(StatManager.GetComponent<FoodStats>().TextureVal + TextureValues[SyrupState]);
        } else {
            Instructions.text = "You can't add anymore pumps!";
        }

        CookingManager.GetComponent<CookingController>().MinigameFinished(3, true);
    }

    //Called by the button to pump syrup
    public void PumpSyrup(){
        if(PumpsLeft > 0){
            StatManager.GetComponent<FoodStats>().AddFlavor(FlavorValues[SyrupState]);
            StatManager.GetComponent<FoodStats>().AddTexture(TextureValues[SyrupState]);
            PumpsLeft--;
        }
    }

    //Called by the buttons to move the cup and change syrups
    //Moves the state left (-1) or right (1)
    public void AdjustSelection(int direction){
        if(SyrupState + direction >= 0 && SyrupState  + direction <= 2){
            SyrupState += direction;
        }
    }

    //This moves the coffee cup to the correct location
    private void MoveCup(){
        Vector3 NewLocation = Vector3.Lerp(Cup.position, SyrupLocations[SyrupState].position, 0.2f);
        Cup.position = NewLocation;
    }


    //This hides and shows pluses based on the 
    private void ShowPluses(){
        StatManager.GetComponent<FoodStats>().ShowPlus(2);
        StatManager.GetComponent<FoodStats>().HidePlus(1);

        if(SyrupState > 0){
            StatManager.GetComponent<FoodStats>().ShowPlus(0);    
        } else {
            StatManager.GetComponent<FoodStats>().HidePlus(0);
        }

    }


    #endregion

    #region Animation Controls

    public void PressedFinished()
    {
    //    Animation.SetBool("Pressed", false);
    }

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
