using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodDrop : MonoBehaviour
{
    public int warmth;
    public int flavor;
    public int texture;
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

    public void setImage(Sprite ingredient){
        this.gameObject.GetComponent<Image>().sprite = ingredient;
    }
}
