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

    #endregion

    #region UI Related Stuff

    /**
    This section is mainly for the UI elements to display what you're at and how much of
    one stat you have.
    **/
    [Header("Bean Previews")]
    public Button BeanButton1;
    public Button BeanButton2;
    public Button BeanButton3;
    public Slider[] Bean1Previews = new Slider[3];
    public Slider[] Bean2Previews = new Slider[3];
    public Slider[] Bean3Previews = new Slider[3];

    [Header("Bars at the Bottom")]
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

    #region Private Vars

    private InventoryTracker Tracker;
    private Ingredient Bean1;
    private Ingredient Bean2;
    private Ingredient Bean3;

    #endregion

    public void Start(){
        GameObject temp = GameObject.FindGameObjectWithTag("InventoryTracker");
        Tracker = temp.GetComponent<InventoryTracker>();

        if(Tracker.inventory[0] != null){
            Bean1 = Tracker.inventory[0];
            //BeanButton1.interactable = true;
        } else {
            Bean1.texture = 0;
            Bean1.warmth = 0;
            Bean1.flavor = 0;
        }
        Bean1 = Tracker.inventory[0];
        if(Tracker.inventory[1] != null){
            Bean2 = Tracker.inventory[1];
            //BeanButton2.interactable = true;
        } else {
            Bean2.texture = 0;
            Bean2.warmth = 0;
            Bean2.flavor = 0;
        }
        if(Tracker.inventory[2] != null){
            Bean3 = Tracker.inventory[2];
            //BeanButton3.interactable = true;
        } else {
            Bean3.texture = 0;
            Bean3.warmth = 0;
            Bean3.flavor = 0;
        }

        SetBeanPickingBars();

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

    #region Bean Previews & More

    /**
        This sets it so that way the beans you pick can show
        what you do so it look pretty and you can consciously pick
        which thing you want
    **/
    private void SetBeanPickingBars(){
        //Import the Textures
        Bean1Previews[0].value = Bean1.texture;
        Bean2Previews[0].value = Bean2.texture;
        Bean3Previews[0].value = Bean3.texture;

        //Import the Warmth
        Bean1Previews[1].value = Bean1.warmth;
        Bean2Previews[1].value = Bean2.warmth;
        Bean3Previews[1].value = Bean3.warmth;

        //Import the Flavor
        Bean1Previews[2].value = Bean1.flavor;
        Bean2Previews[2].value = Bean2.flavor;
        Bean3Previews[2].value = Bean3.flavor;
    }

    public void SelectBean(int BeanNumber){
        switch(BeanNumber){
            case 1:
                TextureVal = Bean1.texture;
                WarmthVal = Bean1.warmth;
                FlavorVal = Bean1.flavor;
            break;
            case 2:
                TextureVal = Bean2.texture;
                WarmthVal = Bean2.warmth;
                FlavorVal = Bean2.flavor;
            break;
            case 3:
                TextureVal = Bean3.texture;
                WarmthVal = Bean3.warmth;
                FlavorVal = Bean3.flavor;
            break;
        }
        
    }

    #endregion

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

    public void DishDone(){
        tracker.foodObject.texture = TextureVal;
        tracker.foodObject.flavor = FlavorVal;
        tracker.foodObject.warmth = WarmthVal;
        tracker.hasFood = true;
    }

    #endregion

}
