using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PouringMinigame : MonoBehaviour{

    #region Public Variables

    [Header("Minigame Objects and Variables")]
    public float TimerCount = 5f;

    [Header("UI and Stat Manager")]
    [Tooltip("This is the manager for the stats so we can update them")]
    public GameObject StatManager;

    [Tooltip("This is the cooking manager so it manages the minigames and order etc")]
    public GameObject CookingManager;

    [Tooltip("This is the instructions so players know what to fuckin do")]
    public Text Instructions;

    #endregion

    #region Private Variables
    
    //This is so that you can't play the game if it's not active
    private bool MinigameActive = false;

    //This is so you can signal to the Cooking Controller that the minigame is complete
    private bool MinigameCompleted = false;

    #endregion

    // Update is called once per frame
    void Update(){
        if(MinigameActive)
            RunMinigame();
    }

    private void RunMinigame(){
        if(!MinigameCompleted){
            CheckPouring();
            UpdateInstructions();
        }

        if(MinigameCompleted){
            CookingManager.GetComponent<CookingController>().MinigameFinished();
        }
    }

    private void CheckPouring(){
        if(Input.GetButton("Use")){
            if(TimerCount > 0){
                TimerCount -= Time.deltaTime;
            } else {
                MinigameCompleted = true;
            }
        }
    }

    /**
        This updates the instructions text so it properly displays what to do and stuff
    **/
    private void UpdateInstructions(){
        if(MinigameCompleted == false){
            Instructions.text = "Hold space to pour the coffee!! Just pretend for now!!!";
        } else {
           Instructions.text = "You did it binch! Click Next to move onto the next step!";
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
