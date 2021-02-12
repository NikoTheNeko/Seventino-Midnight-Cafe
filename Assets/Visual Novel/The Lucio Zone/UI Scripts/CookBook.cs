﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBook : MonoBehaviour
{
    public ingredientPage[] ingredientEntries;
    public page[] enemyEntries;
    public page[] dishEntries;

    private page[] currentSection;
    private int currentEntry;
    // Start is called before the first frame update
    void Start()
    {
        currentEntry = 0;
        currentSection = dishEntries;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnPage(bool direction){
        currentSection[currentEntry].deactivate();
        if(currentEntry < currentSection.Length && direction){
            currentEntry++;
        }
        else if(currentEntry >= 0 && !direction){
            currentEntry--;
        }
        currentSection[currentEntry].activate();
    }

    public void changeSection(string section){
        switch(section){
            case "ingredients":
            currentSection = ingredientEntries;
            break;
            case "enemies":
            currentSection = enemyEntries;
            break;
            case "dish":
            currentSection = dishEntries;
            break;
            default:
            break;
        }
        currentEntry = 0;
    }
}

[System.Serializable]
public class entry{
    public page[] pages;
    public bool[] discovered = new bool[0];
    public void activate(int loc){
        if(loc < pages.Length && discovered[loc]){
            
        }
    }

    public void deactivate(){
        foreach(page temp in pages){
            temp.deactivate();
        }
    }
}

[System.Serializable]
public class page{
    public GameObject[] contents;
    public int pageNum;

    public void activate(){
        foreach(GameObject element in contents){
            element.SetActive(true);
        }
    }

    public void deactivate(){
        foreach(GameObject element in contents){
            element.SetActive(false);
        }
    }
}

public class ingredientPage:page{
     
    public bool[] discovered;

    public class ingredient{
        
    }
}
