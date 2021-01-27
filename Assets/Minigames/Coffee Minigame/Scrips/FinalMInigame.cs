using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalMInigame : MonoBehaviour{

    [Header("UI and Stat Manager")]
    [Tooltip("This is the manager for the stats so we can update them")]
    public GameObject StatManager;

    [Tooltip("This is the cooking manager so it manages the minigames and order etc")]
    public GameObject CookingManager;

    [Tooltip("This is the instructions so players know what to fuckin do")]
    public Text Instructions;

    private bool MinigameActive = false;

    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        if(MinigameActive){
            Instructions.text = "Congrats! You made a coffee. Look at Remy Rattatouie he is happy and stuck free him from his funko prison. Jazz 5";
            CookingManager.GetComponent<CookingController>().MinigameFinished();
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
