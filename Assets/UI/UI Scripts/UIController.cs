using System.Collections;
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
    public List<Name> names;
    public Image toDoName;
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
        int health = player.GetComponent<PlayerCombatTesting>().health;
        // for(int i = 0; i < (int)(health/2); i++)
        while(counter < (int)(health/2)){
            hearts[counter].sprite = fullHeart;
            counter++;
        }
        if(health%2 != 0){
            hearts[counter].sprite = halfHeart;
            counter++;
        }
        while(counter < hearts.Length){
            // hearts[counter].transform.gameObject.SetActive(false);
            hearts[counter].sprite = emptyHeart;
            counter++;
        }
    }

    public void triggerControl(){
        if(tracker.inventory.Count > 0){
            leaveTrigger.SetActive(true);
        }
    }

    public void toDoControl(Dialogue curQuest){
        textureMin.value = curQuest.textureMin;
        textureMax.value = 100 - curQuest.textureMax;
        warmthMin.value = curQuest.warmthMin;
        warmthMax.value = 100 - curQuest.warmthMax;
        flavorMin.value = curQuest.flavorMin;
        flavorMax.value = 100 - curQuest.flavorMax;
        
        foreach(Name name in names){
            if(name.name == curQuest.subject){
                toDoName.sprite = name.sprite;
            }
        }
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

[System.Serializable]
public class Name{
        public string name;
        public Sprite sprite;
    }
