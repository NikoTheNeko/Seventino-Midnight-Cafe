using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodDrop : MonoBehaviour
{
    public int warmth;
    public int flavor;
    public int texture;
    private string ingredientName;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>().getPic(ingredientName);
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
}
