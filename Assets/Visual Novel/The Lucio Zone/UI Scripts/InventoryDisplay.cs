﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    #region Public Variables
    [Tooltip("Texts and Images for displaying the inventory. ")]
    public List<Display> ingredientDisplays = new List<Display>();
    public GameObject mouseFollower;
    public Text followerText;
    public GameObject onScreenAnchor;
    public GameObject offScreenAnchor;
    #endregion

    #region Private Variables
    private int OffsetX = 50; //amount of leeway in x axis
    private int OffsetY = 50; //amount of leeway in y axis
    private InventoryTracker tracker;
    private bool onScreen = false;
    private bool textActive = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("InventoryTracker");
        tracker = temp.GetComponent<InventoryTracker>();
        
        foreach(Display display in ingredientDisplays){
            display.Deactivate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        displayIngredients();
        checkForMouseOver();
        mouseFollower.transform.position = Input.mousePosition;
        followerText.gameObject.SetActive(textActive);
        move();
        if(Input.GetKeyDown(KeyCode.I)){
            onScreen = !onScreen;
        }
    }

    //Displays all discovered ingredients on given display objects
    //If ingredient in ingredientPictures hasn't been discovered, it will be passed over when displaying
    private void displayIngredients(){
        int display = 0;

        foreach(FoodDrop food in tracker.inventory){
            if(display > ingredientDisplays.Count){
                break;
            }
            ingredientDisplays[display].image.sprite = food.image;
            ingredientDisplays[display].Activate();
            ingredientDisplays[display].setVars(food.texture, food.warmth, food.flavor, food.name);
            display++;
        }
    }

    //Checks if mouse has moved over an image
    private void checkForMouseOver(){
        textActive = false;
        foreach(Display target in ingredientDisplays){
            if(Input.mousePosition.x < target.image.gameObject.transform.position.x + OffsetX && 
            Input.mousePosition.x > target.image.gameObject.transform.position.x - OffsetX && 
            Input.mousePosition.y < target.image.gameObject.transform.position.y + OffsetY && 
            Input.mousePosition.y > target.image.gameObject.transform.position.y - OffsetY &&
            target.active){
                textActive = true;
                //info should be displayed here
                followerText.text = "Name: " + target.name + 
                                    "\nFlavor: " + target.flavor + 
                                    "\nTexture: " + target.texture + 
                                    "\nWarmth: " + target.warmth;
            }
        }
    }

    private void move(){
        if(onScreen){
            if(this.gameObject.transform.position.y < onScreenAnchor.transform.position.y){
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z);
            }
        }
        else if(!onScreen){
            if(this.gameObject.transform.position.y > offScreenAnchor.transform.position.y){
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, this.gameObject.transform.position.z);
            }
        }
    }
}

[System.Serializable]
public class Display{
    public Image image;
    public string name;
    public int texture;
    public int warmth;
    public int flavor;
    public bool active = false;

    //Sets all members inactive
    public void Deactivate(){
        image.gameObject.SetActive(false);
        active = false;
    }

    //Sets all members active
    public void Activate(){
        image.gameObject.SetActive(true);
        active = true;
    }

    public void setVars(int iTexture, int iWarmth, int iFlavor, string iName){
        texture = iTexture;
        warmth = iWarmth;
        flavor = iFlavor;
        name = iName;
    }
}