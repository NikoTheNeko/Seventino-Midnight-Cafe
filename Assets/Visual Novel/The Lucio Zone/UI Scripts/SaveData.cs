using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SaveData
{
    public Ingredient[] inventorySave;

    public SaveData(List<Ingredient> inventory){
        inventorySave = new Ingredient[inventory.Count];
        for(int i = 0; i < inventorySave.Length; i++){
            inventorySave[i] = inventory[i];
        }
    }
    
}
