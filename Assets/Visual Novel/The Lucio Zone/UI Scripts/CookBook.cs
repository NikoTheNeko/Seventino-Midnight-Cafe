using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBook : MonoBehaviour
{
    public entry[] ingredientEntries;
    public entry[] enemyEntries;
    public entry[] dishEntries;

    private entry[] currentSection;
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

    //turns page if there are more entries in the array
    //moves in positive direction if direction is true
    //move in negative direction if direction is false
    public void turnPage(bool direction){
        //maybe don't pass currentEntry to deactivate or activate, just put in there for temp
        currentSection[currentEntry].deactivate(currentEntry);
        if(currentEntry < currentSection.Length && direction){
            currentEntry++;
        }
        else if(currentEntry >= 0 && !direction){
            currentEntry--;
        }
        currentSection[currentEntry].activate(currentEntry);
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

//a pair of pages to display when the book is open
[System.Serializable]
public class entry{
    public page[] pages;
    public bool[] discovered = new bool[0];
    public void activate(int loc){
        if(loc < pages.Length && discovered[loc]){
            pages[loc].activate();
        }
    }

    public void deactivate(int loc){
        if(loc < pages.Length && discovered[loc]){
            pages[loc].deactivate();
        }
    }
}

//general page for information that takes up the whole page
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

//page of ingredients and their information
//main difference is ingredientPage can have some info on some info off
[System.Serializable]
public class ingredientPage:page{
    
    public bool[] discovered;
    public ingredient[] contents;

    public void activate(){
        int x = 0;
        while(discovered[x] && x < discovered.Length){
            contents[x].activate();
            x++;
        }
    }

    public void deactivate(){
        int x = 0;
        while(discovered[x] && x < discovered.Length){
            contents[x].deactivate();
            x++;
        }
    }

    public void discover(int loc){
            if(loc > 0 && loc < contents.Length){
                discovered[loc] = true;
            }
        }

    //represents individual ingredient description
    public class ingredient{
        public GameObject[] contents;

         public void activate(){
            foreach(GameObject temp in contents){
                temp.SetActive(true);
            }
        }

        public void deactivate(){
            foreach(GameObject temp in contents){
                temp.SetActive(false);
            }
        }
    }
}
