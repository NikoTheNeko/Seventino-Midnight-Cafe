using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpicesScript : MonoBehaviour{
  #region Public Variables

    [Header("Minigame Objects and Variables")]
    [Tooltip("The amount you will add to Flavor")]
    //Values forr when you add the flavors
    public int[] FlavorValues = {10, 8, 5};

    [Tooltip("The amount you will add to Warmth")]
    public int[] WarmthValues = {0, 2, 5};
    [Tooltip("Amount of Scoops")]
    public int ScoopsLeft = 4;


    //[Tooltip("Spices SFX")]
    public AudioSource SpicesSFX;
   // public Animator Animation;

    [Tooltip("The locations of each Spices bottle")]
    public Transform[] SpicesLocations = new Transform[3];

    public ParticleSystem[] SpicePS = new ParticleSystem[3];

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
    //States for the Spices
    //0 - Vanilla
    //1 - Caramel
    //2 - Chocolate
    private int SpicesState = 0;
    
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
            SpicesState = 0;
        }
    }

    #region Minigame Function
    /**
        Since this minigame is so small I shoved everything into one function.
        It just adds the amount from AddAmount to the thing. That's it.
    **/
    private void RunMinigame(){
        Instructions.text = "Press left and right to select a spice and press add to add!";
        ShowPluses();
        MoveCup();

        if(ScoopsLeft > 0){
            Instructions.text = "Press left and right to select a spice and press add to add!\n" + "Scoops Left: " + ScoopsLeft;
            StatManager.GetComponent<FoodStats>().UpdateTexturePreview(0);
            StatManager.GetComponent<FoodStats>().UpdateWarmthPreview(StatManager.GetComponent<FoodStats>().WarmthVal + WarmthValues[SpicesState]);
            StatManager.GetComponent<FoodStats>().UpdateFlavorPreview(StatManager.GetComponent<FoodStats>().FlavorVal + FlavorValues[SpicesState]);
        } else {
            Instructions.text = "You can't add anymore scoops!";
        }

        CookingManager.GetComponent<CookingController>().MinigameFinished(4, true);
    }

    //Called by the button to pump Spices
    public void PumpSpices(){
        if(ScoopsLeft > 0){
            SpicePS[SpicesState].Play();
            SpicesSFX.Play();
            StatManager.GetComponent<FoodStats>().AddFlavor(FlavorValues[SpicesState]);
            StatManager.GetComponent<FoodStats>().AddWarmth(WarmthValues[SpicesState]);
            ScoopsLeft--;
        }
    }

    //Called by the buttons to move the cup and change Spicess
    //Moves the state left (-1) or right (1)
    public void AdjustSelection(int direction){
        if(SpicesState + direction >= 0 && SpicesState + direction <= 2){
            SpicesState += direction;
        }
    }

    //This moves the coffee cup to the correct location
    private void MoveCup(){
        Vector3 NewLocation = Vector3.Lerp(Cup.position, SpicesLocations[SpicesState].position, 0.2f);
        Cup.position = NewLocation;
    }


    //This hides and shows pluses based on the 
    private void ShowPluses(){
        StatManager.GetComponent<FoodStats>().ShowPlus(2);
        StatManager.GetComponent<FoodStats>().HidePlus(0);

        if(SpicesState > 0){
            StatManager.GetComponent<FoodStats>().ShowPlus(1);    
        } else {
            StatManager.GetComponent<FoodStats>().HidePlus(1);
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
