using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilkMinigame : MonoBehaviour{

    #region Public Variables

    [Header("Minigame Objects and Variables")]
    [Tooltip("The amount you will add to Warmth")]    
    public int AddAmount = 1;
    [Tooltip("Pouring SFX")]
    public AudioSource PouringSFX;

    [Header("UI and Stat Manager")]
    [Tooltip("This is the manager for the stats so we can update them")]
    public GameObject StatManager;

    [Tooltip("This is the cooking manager so it manages the minigames and order etc")]
    public GameObject CookingManager;

    [Tooltip("This is the instructions so players know what to fuckin do")]
    public Text Instructions;
    
    [Tooltip("Slider for the the minigame")]
    public Slider PouringSlider;
    [Tooltip("The handle of the slider so it can tilt")]
    public Transform Pitcher;
    [Tooltip("The angle it will tip at at the very top")]
    public float TiltAngle = 45f;


    #endregion

    #region Private Variables
    
    //This is so that you can't play the game if it's not active
    private bool MinigameActive = false;

    #endregion

    // Update is called once per frame
    void Update(){
        if(MinigameActive){
            PouringSlider.interactable = true;
            StatManager.GetComponent<FoodStats>().ShowPlus(1);
            RunMinigame();
        } else {
            PouringSlider.interactable = false;
            if(PouringSlider.value > 0){
                TiltPitcher();
                PouringSlider.value -= 0.5f * Time.deltaTime;
            }
            StatManager.GetComponent<FoodStats>().HidePlus(1);           
            StatManager.GetComponent<FoodStats>().UpdateWarmthPreview(0);
        }
    }

    #region Minigame Function
    /**
        Since this minigame is so small I shoved everything into one function.
        It just adds the amount from AddAmount to the thing. That's it.
    **/
    private void RunMinigame(){
        Instructions.text = "Click and drag the pitcher up to pour, the higher you pour the faster you pour.";

        StatManager.GetComponent<FoodStats>().UpdateWarmthPreview(StatManager.GetComponent<FoodStats>().WarmthVal + AddAmount * PouringSlider.value);

        if(PouringSlider.value > 0){
            TiltPitcher();
            StatManager.GetComponent<FoodStats>().AddWarmth(AddAmount * PouringSlider.value * Time.deltaTime);
            PouringSFX.Play();
        }
        CookingManager.GetComponent<CookingController>().MinigameFinished();
    }

    private void TiltPitcher(){
        float NewAngle = TiltAngle * PouringSlider.value;
        Pitcher.rotation = Quaternion.AngleAxis(NewAngle, Vector3.forward);
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
