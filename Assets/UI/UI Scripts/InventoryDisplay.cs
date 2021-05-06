using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    #region Public Variables
    [Tooltip("Texts and Images for displaying the inventory. ")]
    public List<Display> ingredientDisplays = new List<Display>();
    public GameObject mouseFollower;
    // public Text followerText;
    public GameObject onScreenAnchor;
    public GameObject offScreenAnchor;
    #endregion

    #region Private Variables
    private int OffsetX = 40; //amount of leeway in x axis
    private int OffsetY = 40; //amount of leeway in y axis
    private InventoryTracker tracker;
    private bool onScreen = false;
    private bool textActive = false;
    private Slider textureSlider;
    private Slider warmthSlider;
    private Slider flavorSlider;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("InventoryTracker");
        tracker = temp.GetComponent<InventoryTracker>();
        
        foreach(Display display in ingredientDisplays){
            display.Deactivate();
        }

        textureSlider = mouseFollower.transform.GetChild(1).gameObject.GetComponent<Slider>();
        warmthSlider = mouseFollower.transform.GetChild(2).gameObject.GetComponent<Slider>();
        flavorSlider = mouseFollower.transform.GetChild(3).gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        displayIngredients();
        checkForMouseOver();
        mouseFollower.transform.position = Input.mousePosition;
        // followerText.gameObject.SetActive(textActive);
        // move();
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

    //Checks if mouse has moved over an image
    private void checkForMouseOver(){
        // textActive = false;
        foreach(Display target in ingredientDisplays){
            if(Input.mousePosition.x < target.image.gameObject.transform.position.x + OffsetX && 
            Input.mousePosition.x > target.image.gameObject.transform.position.x - OffsetX && 
            Input.mousePosition.y < target.image.gameObject.transform.position.y + OffsetY && 
            Input.mousePosition.y > target.image.gameObject.transform.position.y - OffsetY &&
            target.active){
                // textActive = true;
                Debug.Log("went over");
                // textureSlider.transform.gameObject.SetActive(true);
                // warmthSlider.transform.gameObject.SetActive(true);
                // flavorSlider.transform.gameObject.SetActive(true);

                textureSlider.value = target.texture;
                warmthSlider.value = target.warmth;
                flavorSlider.value = target.flavor;
                //info should be displayed here
                // followerText.text = "Name: " + target.name + 
                //                     "\nTexture: " + target.texture + 
                //                     "\nWarmth: " + target.warmth + 
                //                     "\nFlavor: " + target.flavor;
            }
            else{
                // textureSlider.transform.gameObject.SetActive(false);
                // warmthSlider.transform.gameObject.SetActive(false);
                // flavorSlider.transform.gameObject.SetActive(false);
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
    public float texture;
    public float warmth;
    public float flavor;
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

    public void setVars(float iTexture, float iWarmth, float iFlavor, string iName){
        texture = iTexture;
        warmth = iWarmth;
        flavor = iFlavor;
        name = iName;
    }
}