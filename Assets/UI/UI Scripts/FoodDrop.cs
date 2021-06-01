using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodDrop : MonoBehaviour
{
    public float texture;
    public float warmth;
    public float flavor;
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

    public float getWarmth(){
        return warmth;
    }

    public float getFlavor(){
        return flavor;
    }

    public float getTexture(){
        return texture;
    }
    
    public void setWarmth(float input){
        warmth = input;
    }

    public void setFlavor(float input){
        flavor = input;
    }

    public void setTexture(float input){
        texture = input;
    }

    //the apple factory
    public void setValues(float iTexture, float iWarmth, float iFlavor, string iName){
        warmth = iWarmth;
        flavor = iFlavor;
        texture = iTexture;
        name = iName;
    }

    public float[] getValues(){
        return new float[] {texture, warmth, flavor};
    }

    public void setImage(Sprite ingredient){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = ingredient;
        image = ingredient;
    }

    // void OnCollisionEnter(Collision collision){
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            InventoryTracker tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
            if(tracker.inventory.Count < 3){
                tracker.add(this.gameObject.GetComponent<FoodDrop>());
                Destroy(this.gameObject);
            }
            
        }
        
    }
}
