﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilkMinigame : MonoBehaviour{

    #region Public Variables

    [Header("Minigame Objects and Variables")]
    [Tooltip("The max amount you will add to Warmth")]
    public int MaxAmount = 30;
    [Tooltip("Max amount you will add to Warmth")]    
    public int AddAmount = 1;
    [Tooltip("Amount you will add to texture")]
    public float AddTexture = 0.5f;
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
    [Tooltip("Particle System")]
    public ParticleSystem MilkPS;
    [Tooltip("The Progress Bar")]
    public Slider ProgressBar;

    public GameObject FullPitcher;


    #endregion

    #region Private Variables
    
    //This is so that you can't play the game if it's not active
    private bool MinigameActive = false;
    
    //This is how much that has been added
    private float AddedAmount = 0;

    #endregion

    // Update is called once per frame
    void Update(){
        if(MinigameActive){
            PouringSlider.interactable = true;
            RunMinigame();
        } else {
            MilkPS.Stop();
            PouringSlider.interactable = false;
            if(PouringSlider.value > 0){
                TiltPitcher();
                PouringSlider.value -= 3f * Time.deltaTime;
            }

        }
        PouringSFX.volume = PouringSlider.value;
    }

    #region Minigame Function
    /**
        Since this minigame is so small I shoved everything into one function.
        It just adds the amount from AddAmount to the thing. That's it.
    **/
    private void RunMinigame(){
        ControlBars();

        if(AddedAmount <= MaxAmount){
            AddMilk();
            float PercentageFloat = 1 - (AddedAmount / MaxAmount);
            Instructions.text = "Click and drag the pitcher up to pour, the higher you pour the faster you pour.";
            ProgressBar.value = PercentageFloat;
        } else {
            FullPitcher.SetActive(false);
            MilkPS.Stop();
            Instructions.text = "No more Milk Left!";
            PouringSlider.value -= 3f * Time.deltaTime;
            StatManager.GetComponent<FoodStats>().UpdateWarmthPreview(0);
            TiltPitcher();
        }

        CookingManager.GetComponent<CookingController>().MinigameFinished(2, true);
    }

    /**
        This tilts the pitcher the higher you bring it up
    **/
    private void TiltPitcher(){
        float NewAngle = TiltAngle * PouringSlider.value;
        Pitcher.rotation = Quaternion.AngleAxis(NewAngle, Vector3.forward);
    }

    /**
        This adds the milk when the slider goes up weee
    **/
    private void AddMilk(){
        if(PouringSlider.value > 0){
            if(!MilkPS.isPlaying)
                MilkPS.Play();
            TiltPitcher();
            StatManager.GetComponent<FoodStats>().AddWarmth(AddAmount * PouringSlider.value * Time.deltaTime);
            StatManager.GetComponent<FoodStats>().AddTexture(AddTexture * PouringSlider.value * Time.deltaTime);
            AddedAmount += AddAmount * PouringSlider.value * Time.deltaTime;
        } else if(PouringSlider.value == 0) {
            if(MilkPS.isPlaying)
                MilkPS.Stop();
        }
    }

    #endregion

    #region Cooking Controller Actions
    private void ControlBars(){
            StatManager.GetComponent<FoodStats>().UpdateWarmthPreview(StatManager.GetComponent<FoodStats>().WarmthVal + AddAmount * PouringSlider.value);
            StatManager.GetComponent<FoodStats>().UpdateTexturePreview(StatManager.GetComponent<FoodStats>().TextureVal + AddTexture * PouringSlider.value);
            StatManager.GetComponent<FoodStats>().ShowPlus(0);  
            StatManager.GetComponent<FoodStats>().ShowPlus(1); 
            StatManager.GetComponent<FoodStats>().HidePlus(2);         
    }
    
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
