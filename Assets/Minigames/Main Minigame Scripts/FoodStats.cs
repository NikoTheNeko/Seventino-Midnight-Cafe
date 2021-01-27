using System.Collections;
using System.Collections.Generic;
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
    public int TextureVal = 0;

    [Tooltip("Warmth value, this checks the warmth of the food, affected by flambethrower")]
    public int WarmthVal = 0;

    [Tooltip("Flavor value, this checks the flavor of the food, affected by gun")]
    public int FlavorVal = 0;

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

    #endregion

    #endregion

    #region Add to stat functions
    /**
    All the functions below here are meant to only add values by getting the component
    and then calling this function. You could probably just add to the value but this just
    makes life easier to manipulate these. They cannot exceed 100
    **/
    public void AddTexture(int amount){
        if(TextureVal < 100)
            TextureVal += amount;
        if(TextureVal > 100)
            TextureVal = 100;
    }

    public void AddWarmth(int amount){
        if(WarmthVal < 100)
            WarmthVal += amount;
        if(WarmthVal > 100)
            WarmthVal = 100;
    }

    public void AddFlavor(int amount){
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

    public void UpdateTexturePreview(int amount){
        TexturePreview.value = amount;
    }

    public void UpdateWarmthPreview(int amount){
        WarmthPreview.value = amount;
    }

    public void UpdateFlavorPreview(int amount){
        FlavorPreview.value = amount;
    }

    #endregion

    #endregion

}
