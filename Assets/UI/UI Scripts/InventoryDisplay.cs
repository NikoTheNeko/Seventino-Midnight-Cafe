using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    #region Public Variables
    [Tooltip("Texts and Images for displaying the inventory. ")]
    public List<Display> ingredientDisplays = new List<Display>();
    #endregion

    #region Private Variables
    private InventoryTracker tracker;
    private bool onScreen = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        
        foreach(Display display in ingredientDisplays){
            display.Deactivate();
        }

    }

    // Update is called once per frame
    void Update()
    {
        displayIngredients();
        // checkForMouseOver();
        if(Input.GetKeyDown(KeyCode.I)){
            onScreen = !onScreen;
        }
    }

    //Displays all discovered ingredients on given display objects
    //If ingredient in ingredientPictures hasn't been discovered, it will be passed over when displaying
    private void displayIngredients(){
        int display = 0;
        
        foreach(Ingredient food in tracker.inventory){
            //note to self: "new text" error maybe caused when too many ingredients in tracker. breaks here, doesn't do checkformouseover
            if(display > ingredientDisplays.Count - 1){
                break;
            }
            ingredientDisplays[display].image.sprite = tracker.getIngredient(food.name).picture;
            ingredientDisplays[display].Activate();
            ingredientDisplays[display].setVars(food.texture, food.warmth, food.flavor, food.name);
            display++;
        }
    }
}

[System.Serializable]
public class Display{
    public Image image;
    public string name;
    public float texture;
    public float warmth;
    public float flavor;
    public bool active = false;
    public Slider textureSlider;
    public Slider warmthSlider;
    public Slider flavorSlider;

    //Sets all members inactive
    public void Deactivate(){
        image.gameObject.SetActive(false);
        textureSlider.transform.gameObject.SetActive(false);
        warmthSlider.transform.gameObject.SetActive(false);
        flavorSlider.transform.gameObject.SetActive(false);
        active = false;
    }

    //Sets all members active
    public void Activate(){
        image.gameObject.SetActive(true);
        textureSlider.transform.gameObject.SetActive(true);
        warmthSlider.transform.gameObject.SetActive(true);
        flavorSlider.transform.gameObject.SetActive(true);
        active = true;
    }

    public void setVars(float iTexture, float iWarmth, float iFlavor, string iName){
        textureSlider.value = iTexture;
        warmthSlider.value = iWarmth;
        flavorSlider.value = iFlavor;
    }
}