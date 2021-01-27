using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyrupScript : MonoBehaviour{
  #region Public Variables

    [Header("Minigame Objects and Variables")]
    [Tooltip("The amount you will add to Flavor")]
    public int AddAmount = 10;

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

    #endregion


    void Start() {
    }

    // Update is called once per frame
    void Update(){
        if(MinigameActive){
            RunMinigame();
        } else {
            StatManager.GetComponent<FoodStats>().UpdateFlavorPreview(0);
        }
    }

    #region Minigame Function
    /**
        Since this minigame is so small I shoved everything into one function.
        It just adds the amount from AddAmount to the thing. That's it.
    **/
    private void RunMinigame(){
        Instructions.text = "Press space to add Vanilla! To continue press next!";

        StatManager.GetComponent<FoodStats>().UpdateFlavorPreview(StatManager.GetComponent<FoodStats>().FlavorVal + AddAmount);

        if(Input.GetButtonDown("Use"))
            StatManager.GetComponent<FoodStats>().AddFlavor(AddAmount);

        CookingManager.GetComponent<CookingController>().MinigameFinished();
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
