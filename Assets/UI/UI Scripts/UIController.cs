﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameObject player;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public GameObject inventoryDisplay;
    public GameObject leaveTrigger;
    public GameObject toDoScreen;
    private InventoryTracker tracker;
    public Slider textureMin;
    public Slider textureMax;
    public Slider warmthMin;
    public Slider warmthMax;
    public Slider flavorMin;
    public Slider flavorMax;
    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryDisplay.SetActive(false);
        toDoScreen.SetActive(false);
        leaveTrigger.SetActive(false);

        toDoControl(tracker.dialogues[tracker.dialogueProg]);
        
    }

    // Update is called once per frame
    void Update()
    {
        healthControl();
        triggerControl();
    }

    public void healthControl(){
        int counter = 0;
        int health = player.GetComponent<PlayerCombatTesting>().health/10;
        while(counter < (int)(health/2)){
            hearts[counter].sprite = fullHeart;
            counter++;
        }
        if(health%2 != 0){
            hearts[counter].sprite = halfHeart;
            counter++;
        }
        while(counter < hearts.Length){
            hearts[counter].sprite = emptyHeart;
            counter++;
        }
    }

    public void staminaControl(){
        float stamina;
    }

    public void triggerControl(){
        
        if(tracker.inventory.Count > 0){
            leaveTrigger.SetActive(true);
        }
    }

    public void toDoControl(Dialogue curQuest){
        int[] foodVals = new int[6];
        textureMin.value = curQuest.textureMin;
        textureMax.value = curQuest.textureMax;
        warmthMin.value = curQuest.warmthMin;
        warmthMax.value = curQuest.warmthMax;
        flavorMin.value = curQuest.flavorMin;
        flavorMax.value = curQuest.flavorMax;
    }

    public void switchInventory(){
        inventoryDisplay.SetActive(!inventoryDisplay.active);
        if(inventoryDisplay.active == true){
            Time.timeScale = 0f;
        }
        else{
            Time.timeScale = 1f;
        }
    }

    public void switchToDo(){
        toDoScreen.SetActive(!toDoScreen.active);
        if(toDoScreen.active == true){
            Time.timeScale = 0f;
        }
        else{
            Time.timeScale = 1f;
        }
    }
}