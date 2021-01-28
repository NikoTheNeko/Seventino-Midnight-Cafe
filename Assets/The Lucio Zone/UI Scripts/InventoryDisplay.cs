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
    private int OffsetX = 10; //amount of leeway in x axis
    private int OffsetY = 10; //amount of leeway in y axis
    private InventoryTracker tracker;
    private bool onScreen = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.Find("InventoryTracker");
        tracker = temp.GetComponent<InventoryTracker>();
        Debug.Log(tracker);
        Debug.Log(tracker.discovered("banana"));
        foreach(Display display in ingredientDisplays){
            display.Deactivate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        displayIngredients();
        checkForMouseOver();
        move();
        if(Input.GetKeyDown(KeyCode.I)){
            Debug.Log("I depressed " + Time.time + " " + onScreen);
            onScreen = !onScreen;
        }
        Debug.Log("y = " + this.gameObject.transform.position.y);
    }

    //Displays all discovered ingredients on given display objects
    //If ingredient in ingredientPictures hasn't been discovered, it will be passed over when displaying
    private void displayIngredients(){
        int ingredient = 0;
        int display = 0;
        while(ingredient < tracker.ingredientPictures.Count && display < ingredientDisplays.Count){

            //if the player has discovered an ingredient display it
            if(tracker.discovered(tracker.ingredientPictures[ingredient].name)){
                ingredientDisplays[display].Activate();
                ingredientDisplays[display].image.sprite = tracker.ingredientPictures[ingredient].picture;
                ingredientDisplays[display].name.text = tracker.ingredientPictures[ingredient].name;
                ingredientDisplays[display].amount.text = "x" +  tracker.getAmount(tracker.ingredientPictures[ingredient].name).ToString();
                display++;
            }
            ingredient++;
        }
    }

    //Checks if mouse has moved over an image
    private void checkForMouseOver(){
        foreach(Display target in ingredientDisplays){
            if(Input.mousePosition.x < target.image.gameObject.transform.position.x + OffsetX && 
            Input.mousePosition.x > target.image.gameObject.transform.position.x - OffsetX && 
            Input.mousePosition.y < target.image.gameObject.transform.position.y + OffsetY && 
            Input.mousePosition.y > target.image.gameObject.transform.position.y - OffsetY){
                //info should be displayed here
                Debug.Log("you moused over " + target.name.text + "!");
            }
        }
    }

    private void move(){
        if(onScreen){
            if(this.gameObject.transform.position.y < 450){
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z);
            }
        }
        else if(!onScreen){
            if(this.gameObject.transform.position.y > 0){
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, this.gameObject.transform.position.z);
            }
        }
    }
}

[System.Serializable]
public class Display{
    public Text name;
    public Text amount;
    public Image image;

    //Sets all members inactive
    public void Deactivate(){
        name.gameObject.SetActive(false);
        amount.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
    }

    //Sets all members active
    public void Activate(){
        name.gameObject.SetActive(true);
        amount.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
    }
}