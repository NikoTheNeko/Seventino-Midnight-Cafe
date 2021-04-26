using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class FoodStats : MonoBehaviour{
    #region Public Vars

    #region Stat Values
    /**
    This region contians the stat values for the food.
    **/

    [Header("The Food Stat Values")]
    [Tooltip("Texture value, this checks the texture of the food, affected by knife")]
    public float TextureVal = 0;

    [Tooltip("Warmth value, this checks the warmth of the food, affected by flambethrower")]
    public float WarmthVal = 0;

    [Tooltip("Flavor value, this checks the flavor of the food, affected by gun")]
    public float FlavorVal = 0;

    private InventoryTracker tracker;

    #endregion

    #region UI Related Stuff

    /**
    This section is mainly for the UI elements to display what you're at and how much of
    one stat you have.
    **/
    [Header("UI Elements")]
    public Slider TextureProgress;
    public Slider TexturePreview;
    public Slider WarmthProgress;
    public Slider WarmthPreview;
    public Slider FlavorProgress;
    public Slider FlavorPreview;

    [Header("Plus UI")]
    public Transform[] ShowLocations = new Transform[3];
    public Transform[] HideLocations = new Transform[3];
    public Transform[] Pluses = new Transform[3];
    public float TransitionSpeed = 0.25f;

    #endregion

    #endregion

    public void Start(){
        GameObject temp = GameObject.FindGameObjectWithTag("InventoryTracker");
        tracker = temp.GetComponent<InventoryTracker>();

        Ingredient importFood = tracker.inventory[0];
        // UnityEngine.Debug.Log(importFood.texture);

        // FoodDrop importFood = tracker.inventory[0];

        TextureVal = importFood.texture;
        WarmthVal = importFood.warmth;
        FlavorVal = importFood.flavor;

        // tracker.remove(tracker.inventory[0]);

    }

    #region Add to stat functions
    /**
    All the functions below here are meant to only add values by getting the component
    and then calling this function. You could probably just add to the value but this just
    makes life easier to manipulate these. They cannot exceed 100
    **/
    public void AddTexture(float amount){
        if(TextureVal < 100)
            TextureVal += amount;
        if(TextureVal > 100)
            TextureVal = 100;
    }

    public void AddWarmth(float amount){
        if(WarmthVal < 100)
            WarmthVal += amount;
        if(WarmthVal > 100)
            WarmthVal = 100;
    }

    public void AddFlavor(float amount){
        if(FlavorVal < 100)
            FlavorVal += amount;
        if(FlavorVal > 100)
            FlavorVal = 100;
    }

    #endregion

    private void Update() {
        //tempDisplay();
        UpdateProgressBars();
    }

    #region Bar UI

    #region Progress Bars

    private void UpdateProgressBars(){
        TextureProgress.value = TextureVal;    
        WarmthProgress.value = WarmthVal;   
        FlavorProgress.value = FlavorVal;           
    }

    #endregion

    #region Preview Bars
    //All of these updates the lighter brown preview bar to indicate how much you will add onto the dish.
    //They're all called by the minigames

    public void UpdateTexturePreview(float amount){
        TexturePreview.value = amount;
    }

    public void UpdateWarmthPreview(float amount){
        WarmthPreview.value = amount;
    }

    public void UpdateFlavorPreview(float amount){
        FlavorPreview.value = amount;
    }

    #endregion

    #endregion

    #region Plus Indicators
    /**
        The array is ordered in this order
        0 - Texture
        1 - Warmth
        2 - Flavor
    **/

    /**
        This move the plus to show what is being affected
    **/
    public void ShowPlus(int StatNum){
        Vector3 PlusLocation = Pluses[StatNum].position;
        Vector3 ShowLocationsVec = ShowLocations[StatNum].position;
        Vector3 NewLoc = Vector3.Lerp(PlusLocation, ShowLocationsVec, TransitionSpeed);
        Pluses[StatNum].position = NewLoc;
    }

    /**
        This hides the plus
    **/
    public void HidePlus(int StatNum){
        Vector3 PlusLocation = Pluses[StatNum].position;
        Vector3 HideLocationsVec = HideLocations[StatNum].position;
        Vector3 NewLoc = Vector3.Lerp(PlusLocation, HideLocationsVec, TransitionSpeed);
        Pluses[StatNum].position = NewLoc;
    }

    #endregion

    #region Check Stats
    /**
        This gives you an array of the values for the 
    **/
    public float[] GetValues(){
        float[] ValueArray = {TextureVal, WarmthVal, FlavorVal};
        return ValueArray;
    }

    
    /**
        This function checks the values and returns if it is in the range specified.
    **/
    public bool CheckValues(float TextureTarget,float WarmthTarget,float FlavorTarget,float GraceRange){
        bool TexturePass = false;
        bool WarmthPass = false;
        bool FlavorPass = false;

        TexturePass = IsInRange(TextureVal, TextureTarget, GraceRange);
        WarmthPass = IsInRange(WarmthVal, WarmthTarget, GraceRange);
        FlavorPass = IsInRange(FlavorVal, FlavorTarget, GraceRange);

        if(TexturePass && WarmthPass && FlavorPass){
            return true;
        } else {
            return false;
        }

    }

    /**
        Helper function for Check Values. This just goes in and calculates everything
        so I don't have to copy and paste it over and over again
    **/
    private bool IsInRange(float ValueCheck, float Target, float Range){
        if(ValueCheck >= (Target - Range) && ValueCheck <= (Target + Range)){
            return true;
        } else {
            return false;
        }
    }

    #endregion

}
