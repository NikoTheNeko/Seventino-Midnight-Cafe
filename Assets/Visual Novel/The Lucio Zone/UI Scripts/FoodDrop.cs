using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodDrop : MonoBehaviour
{
    public int texture;
    public int warmth;
    public int flavor;
    public string name;
    public Sprite image;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getWarmth(){
        return warmth;
    }

    public int getFlavor(){
        return flavor;
    }

    public int getTexture(){
        return texture;
    }
    
    public void setWarmth(int input){
        warmth = input;
    }

    public void setFlavor(int input){
        flavor = input;
    }

    public void setTexture(int input){
        texture = input;
    }

    //the apple factory
    public void setValues(int iTexture, int iWarmth, int iFlavor, string iName){
        warmth = iWarmth;
        flavor = iFlavor;
        texture = iTexture;
        name = iName;
    }

    public void setImage(Sprite ingredient){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = ingredient;
    }

    // void OnCollisionEnter(Collision collision){
    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("collided");
        if(other.gameObject.tag == "Player"){
            InventoryTracker tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
            tracker.add(this.gameObject.GetComponent<FoodDrop>());
            Destroy(this.gameObject);
        }
        
    }
}
