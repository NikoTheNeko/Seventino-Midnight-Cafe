using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class InventoryTracker : MonoBehaviour
{
    #region Public Variables
    [Tooltip("Takes in a Sprite and a name associated with that Sprite. The name should be the same as key used in inventoryDict. Place in order of appearance in inventory menu.")]
    public List<IngredientPicture> ingredientPictures = new List<IngredientPicture>();
    [Tooltip("Spawnable Food prefab")]
    public FoodDrop foodObject;
    public List<Ingredient> inventory = new List<Ingredient>();

    public TextAsset[] TextFiles;

    public List<Dialogue> dialogues;
    public int dialogueProg = 0;
    public bool hasFood;
    

    #endregion

    // Start is called before the first frame update

    void Awake(){
        //convert all given text assets to dialogue structs
        foreach(TextAsset asset in TextFiles){
            dialogues.Add(JsonUtility.FromJson<Dialogue>(asset.text));
        }
    }
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

    //Saves current inventory state to savedData.smc
    public void save(){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedData.smc";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        SaveData data = new SaveData(inventory, dialogueProg);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    //Loads inventory state from savedData.smc
    public void load(){

        string path = Application.persistentDataPath + "/savedData.smc";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData load = formatter.Deserialize(stream) as SaveData;

            inventory.Clear();
            foreach(Ingredient ingredient in load.inventorySave){
                inventory.Add(ingredient);
            }
        }
        else{
            Debug.LogError("save file not found");
        }
    }
}

[System.Serializable]
public class IngredientPicture{
    public string name;
    public Sprite picture;
}

[System.Serializable]
public class Ingredient{
    public string name;
    public int texture;
    public int warmth;
    public int flavor;

    public Ingredient(FoodDrop food){
        name = food.name;
        int[] vars = food.getValues();
        texture = vars[0];
        warmth = vars[1];
        flavor = vars[2];
    }
}