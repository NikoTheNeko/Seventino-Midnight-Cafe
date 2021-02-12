using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InventoryTracker : MonoBehaviour
{
    #region Public Variables
    [Tooltip("Takes in a Sprite and a name associated with that Sprite. The name should be the same as key used in inventoryDict. Place in order of appearance in inventory menu.")]
    public List<Ingredient> ingredients = new List<Ingredient>();
    [Tooltip("Spawnable Food prefab")]
    public GameObject foodObject;
    #endregion
    //dictionary of objects being stored
    //key is name of ingredient
    //value is number of ingredient in inventory
    private Dictionary<string, int> inventoryDict = new Dictionary<string, int>();
    

    // Start is called before the first frame update
    void Start()
    {
        //destroys self if it is a duplicate
        GameObject[] search = GameObject.FindGameObjectsWithTag("InventoryTracker");
        if(search.Length > 1){
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //returns the amount of an ingredient
    //returns -1 if the ingredient has not been discovered
    public int getAmount(string ingredient){
        if(inventoryDict.ContainsKey(ingredient)){
            return inventoryDict[ingredient];
        }
        else{
            return -1;
        }
    }

    //subtracts modifier from amount in inventory linked to key
    //returns true if subtraction successful
    //returns false if there aren't enough ingredients or ingredient doesn't exist
    public bool subtract(string ingredient, int modifier){
        //check if ingredient has been found
        if(inventoryDict.ContainsKey(ingredient)){
            int temp = inventoryDict[ingredient];
            temp -= modifier;
            if(temp < 0){
                return false;
            }
            //can't subtract
            else{
                inventoryDict[ingredient] = temp;
                return true;
            }
        }
        else{
            return false;
        }
    }

    //adds modifier to amount in inventory linked to key
    //returns true if addition succesful
    //returns false if addition couldn't be done
    public bool add(string ingredient, int modifier){
        //check if ingredient has been found
        if(inventoryDict.ContainsKey(ingredient)){
            int temp = inventoryDict[ingredient];
            temp += modifier;
            
            //if addition results in a negative return false
            if(temp < 0){
                return false;
            }
            //else do addition
            else{
                inventoryDict[ingredient] = temp;
                return true;
            }
        }
        //adds ingredient to inventoryDict, discovering ingredient
        else{
            inventoryDict[ingredient] = modifier;
            return false;
        }
    }

    //Adds 1 to the amount of the ingredient indicated by key
    public void addOne(string ingredient){
        //check if ingredient has been found
        if(inventoryDict.ContainsKey(ingredient)){
            int temp = inventoryDict[ingredient];
            temp += 1;
            if(temp < 0){

            }
            else{
                inventoryDict[ingredient] = temp;
            }
        }
        else{
            inventoryDict[ingredient] = 1;
        }
    }

    //returns true if dictionary already has given key
    //returns false if key not found
    public bool discovered(string ingredient){
        return inventoryDict.ContainsKey(ingredient);
    }

    //returns a reference to a ingredient object
    //returns null if ingredient not found
    public Ingredient getIngredient(string ingredientName){
        
        foreach(Ingredient ingredient in ingredients){
            if(ingredient.name == ingredientName){
                return ingredient;
            }
        }
        return null;
    }
    
    //spawns a foodObject at the given coordinates with the sprite of the given ingredient
    //returns false if the object wasn't spawned
    public bool spawnFood(string ingredient, Vector3 coords){
        if(discovered(ingredient)){
            GameObject food = Instantiate(foodObject, coords, Quaternion.identity);
            Ingredient temp = getIngredient(ingredient);
            food.GetComponent<FoodDrop>().setImage(temp.picture);
            food.GetComponent<FoodDrop>().setValues(temp.warmth, temp.flavor, temp.texture, ingredient);
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class Ingredient{
    public Sprite picture;
    public string name;
    public int texture;
    public int warmth;
    public int flavor;
}

