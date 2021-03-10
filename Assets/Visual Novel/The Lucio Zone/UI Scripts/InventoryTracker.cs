using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InventoryTracker : MonoBehaviour
{
    #region Public Variables
    [Tooltip("Takes in a Sprite and a name associated with that Sprite. The name should be the same as key used in inventoryDict. Place in order of appearance in inventory menu.")]
    public List<IngredientPicture> ingredientPictures = new List<IngredientPicture>();
    [Tooltip("Spawnable Food prefab")]
    public FoodDrop foodObject;
    public List<Ingredient> inventory = new List<Ingredient>();
    

    #endregion

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

    //adds modifier to amount in inventory linked to key
    //returns true if addition succesful
    //returns false if addition couldn't be done
    public void add(FoodDrop food){
        Ingredient temp = new Ingredient(food);
        inventory.Add(temp);
    }

    public void remove(FoodDrop food){
        Ingredient temp = new Ingredient(food);
        inventory.Remove(temp);
    }

    //returns a reference to a ingredient object
    //returns null if ingredient not found
    public IngredientPicture getIngredient(string ingredientName){
        
        foreach(IngredientPicture ingredient in ingredientPictures){
            if(ingredient.name == ingredientName){
                return ingredient;
            }
        }
        return null;
    }
    
    //spawns a foodObject at the given coordinates with the sprite of the given ingredient
    //returns false if the object wasn't spawned
    public bool spawnFood(string name, int texture, int warmth, int flavor, Vector3 coords){
        
        FoodDrop dropped = Instantiate(foodObject, coords, Quaternion.identity);
        dropped.setImage(getIngredient(name).picture);
        dropped.setValues(texture, warmth, flavor, name);
        return false;
    }
}

[System.Serializable]
public class IngredientPicture{
    public Sprite picture;
    public string name;
}

[System.Serializable]
public class Ingredient{
    public Sprite image;
    public string name;
    public int texture;
    public int warmth;
    public int flavor;

    public Ingredient(FoodDrop food){
        image = food.image;
        name = food.name;
        int[] vars = food.getValues();
        texture = vars[0];
        warmth = vars[1];
        flavor = vars[2];
    }
}

