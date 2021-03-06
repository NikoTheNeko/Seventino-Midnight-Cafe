﻿using System.Collections;
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
    public int PumpsLeft = 4;

    public Slider VanillaPump;

    public Slider CaramelPump;
    public Slider ChocolatePump;

    [Tooltip("Syrup SFX")]
    public AudioSource PumpSFX;

    public ParticleSystem[] SyrupPS = new ParticleSystem[3];

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

    public Text LeftText;

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
            ResetSlider(VanillaPump);
            ResetSlider(CaramelPump);
            ResetSlider(ChocolatePump);
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
        AdjustSliders();
        if(PumpsLeft > 0){
            Instructions.text = "Press left and right to select a syrup! Grab and pump to add syrup.";
            LeftText.text = "Left: " + PumpsLeft;
            StatManager.GetComponent<FoodStats>().UpdateFlavorPreview(StatManager.GetComponent<FoodStats>().FlavorVal + FlavorValues[SyrupState]);
            StatManager.GetComponent<FoodStats>().UpdateWarmthPreview(0);
            StatManager.GetComponent<FoodStats>().UpdateTexturePreview(StatManager.GetComponent<FoodStats>().TextureVal + TextureValues[SyrupState]);
        } else {
            LeftText.text = "Left: " + PumpsLeft;
            Instructions.text = "You can't add anymore pumps!";
        }

        CookingManager.GetComponent<CookingController>().MinigameFinished(3, true);
    }

    //Called by the button to pump syrup
    public void PumpSyrup(){
        if(PumpsLeft > 0){
            SyrupPS[SyrupState].Play();
            PumpSFX.Play();
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
        StatManager.GetComponent<FoodStats>().ShowPlus(0);    
        StatManager.GetComponent<FoodStats>().ShowPlus(2);
        StatManager.GetComponent<FoodStats>().HidePlus(1);

    }

    public void AdjustSliders(){

        switch(SyrupState){
            case 0:
                VanillaPump.interactable = true;
                CaramelPump.interactable = false;
                ResetSlider(CaramelPump);
                ResetSlider(ChocolatePump);
                PumpSlider(VanillaPump);
                ChocolatePump.interactable = false;
                break;
            case 1:
                VanillaPump.interactable = false;
                CaramelPump.interactable = true;
                ResetSlider(VanillaPump);
                ResetSlider(ChocolatePump);
                PumpSlider(CaramelPump);
                ChocolatePump.interactable = false;
            break;
            case 2:
                ResetSlider(VanillaPump);
                ResetSlider(CaramelPump);
                PumpSlider(ChocolatePump);
                ChocolatePump.interactable = true;
            break;
        }
    }
    bool Pumped = false;
    public void PumpSlider(Slider Pump){
        if(Pumped){
            ResetSlider(Pump);
            if(Pump.value == 0)
                Pumped = false;
        } else if(Pump.value == 1){
            if(PumpsLeft > 0 && Pumped == false){
                SyrupPS[SyrupState].Play();
                Pump.value -= 0.1f;
                Pumped = true;
                PumpSFX.Play();
                StatManager.GetComponent<FoodStats>().AddFlavor(FlavorValues[SyrupState]);
                StatManager.GetComponent<FoodStats>().AddTexture(TextureValues[SyrupState]);
                PumpsLeft--;
            }
            Pump.interactable = false;
        }
    }

    public void ResetSlider(Slider Pump){
        if(Pump.value > 0)
            Pump.value -= 5f * Time.deltaTime;
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
