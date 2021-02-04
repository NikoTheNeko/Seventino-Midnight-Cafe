using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodDrop : MonoBehaviour
{
    public int warmth;
    public int flavor;
    public int texture;
    public string name;
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
    public void setValues(int iWarmth, int iFlavor, int iTexture, string iName){
        warmth = iWarmth;
        flavor = iFlavor;
        texture = iTexture;
        name = iName;
    }

    public void setImage(Sprite ingredient){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = ingredient;
    }

    // void OnCollisionEnter(Collision collision){
    void OnTriggerEnter(Collider other){
        Debug.Log("collided");
        InventoryTracker tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        tracker.add(name, 1);
        Debug.Log(name + " amount = " + tracker.getAmount(name));
        Destroy(this.gameObject);
    }
}
