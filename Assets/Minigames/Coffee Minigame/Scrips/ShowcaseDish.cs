using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowcaseDish : MonoBehaviour{

    [Header("UI and Stat Manager")]
    [Tooltip("This is the manager for the stats so we can update them")]
    public GameObject StatManager;

    [Tooltip("This is the cooking manager so it manages the minigames and order etc")]
    public GameObject CookingManager;

    [Tooltip("This is the instructions so players know what to fuckin do")]
    public Text Instructions;

    [Tooltip("Audio for the plate")]
    public AudioSource PlateSFX;

    [Header("Visual Novel Segment")]
    [Tooltip("The Canvas for the VN")]
    public GameObject CookingUI;
    [Tooltip("The Canvas for the minigame")]
    public GameObject VNUI;
    [Tooltip("The Good Ending")]
    public GameObject GoodEnding;
    [Tooltip("The Best Ending")]
    public GameObject BestEnding;


    private bool MinigameActive = false;

    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){

        if(MinigameActive){
            Instructions.text = "Congrats! You made a coffee! Press space to serve the dish!";
            if(StatManager.GetComponent<FoodStats>().CheckValues(50,50,50,10)){
                if(Input.GetButtonDown("Use")){
                    VNUI.SetActive(true);
                    CookingUI.SetActive(false);
                    BestEnding.SetActive(true);
                    BestEnding.GetComponent<TextBoxScript>().ActivateObjects();
                }
            } else {
                if(Input.GetButtonDown("Use")){
                    VNUI.SetActive(true);
                    CookingUI.SetActive(false);
                    GoodEnding.SetActive(true);
                    GoodEnding.GetComponent<TextBoxScript>().ActivateObjects();
                }
            }
            BestEnding.GetComponent<TextBoxScript>().ActivateObjects();
            GoodEnding.GetComponent<TextBoxScript>().ActivateObjects();

            CookingManager.GetComponent<CookingController>().MinigameFinished();
        }
        
    }


    
    #region Cooking Controller Actions
    //ActivateMinigame gets called to activate the minigame so it can actually run
    public void ActivateMinigame(){
        MinigameActive = true;
        PlateSFX.Play();
    }

    //DeactivateMinigame turns the minigame off so you can't play it
    public void DeactivateMinigame(){
        MinigameActive = false;
    }
    #endregion
}
