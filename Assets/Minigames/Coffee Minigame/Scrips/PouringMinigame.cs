using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PouringMinigame : MonoBehaviour{

    #region Public Variables

    [Header("Minigame Objects and Variables")]
    [Tooltip("How long the minigame will last")]
    public float TimerCount = 5f;

    [Tooltip("Pouring SFX")]
    public AudioSource PouringSFX;

    [Header("UI and Stat Manager")]
    [Tooltip("This is the manager for the stats so we can update them")]
    public GameObject StatManager;

    [Tooltip("This is the cooking manager so it manages the minigames and order etc")]
    public GameObject CookingManager;

    [Tooltip("This is the instructions so players know what to fuckin do")]
    public Text Instructions;

    [Tooltip("This is the mask for the coffee so you can't see it")]
    public Transform MaskingImage;

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

    //This is so you can signal to the Cooking Controller that the minigame is complete
    private bool MinigameCompleted = false;

    //This is a nondestructive way for the timer to not fucking die and get destroyed
    private float PourTime;

    //This is the size of the mask so that way it can fill up
    private float MaskSize = 3.7f;

    //This keeps track of how much of the percentage has been done
    private float PercentDone = 0.0f;

    #endregion

    private void Start() {
        PourTime = TimerCount;
    }

    // Update is called once per frame
    void Update(){
        if(MinigameActive){
            RunMinigame();
        } else {
            if(PouringSlider.value > 0){
                TiltPitcher();
                PouringSlider.value -= 0.5f * Time.deltaTime;
            }
        }
    }

    private void RunMinigame(){
        if(!MinigameCompleted){
            CheckPouring();
            UpdateInstructions();
        }

        if(MinigameCompleted){
            PouringSlider.interactable = false;
            if(PouringSlider.value > 0)
                PouringSlider.value -= 0.5f * Time.deltaTime;
            TiltPitcher();
            PouringSFX.Stop();
            CookingManager.GetComponent<CookingController>().MinigameFinished();
        }
    }

    private void CheckPouring(){
        if(PouringSlider.value >= 1){
            if(PourTime > 0){
                PouringSFX.mute = false;
                PourTime -= Time.deltaTime;
                UpdatePercentage();
                MoveMaskingImage();
            } else {
                MinigameCompleted = true;
            }
        } else if(PouringSlider.value < 1){
            PouringSFX.mute = true;
        }

        TiltPitcher();
    }

    private void TiltPitcher(){
        float NewAngle = TiltAngle * PouringSlider.value;
        Pitcher.rotation = Quaternion.AngleAxis(NewAngle, Vector3.forward);
    }

    private void UpdatePercentage(){
        PercentDone = (TimerCount - PourTime) / TimerCount;
    }

    public void MoveMaskingImage(){
        Vector3 NewSize = new Vector3(1, PercentDone * 1.5f, 0);
        MaskingImage.localScale = NewSize;
    }

    /**
        This updates the instructions text so it properly displays what to do and stuff
    **/
    private void UpdateInstructions(){
        if(MinigameCompleted == false){
            Instructions.text = "Click and drag the kettle up all the way to brew the coffee!";
        } else {
           Instructions.text = "You did it! Click Next to move onto the next step!";
        }
    }

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
