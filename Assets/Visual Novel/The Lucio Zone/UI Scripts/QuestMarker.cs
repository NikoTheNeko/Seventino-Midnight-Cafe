using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMarker : MonoBehaviour
{
    public GameObject marker;
    public TextBoxScript textbox;
    public GameObject trigger;
    bool entered = false;
    public TextAsset[] idleText;
    InventoryTracker tracker;
    Dialogue curQuest;
    public string subject;
    private int idlePos = -1;

    private bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
        marker.SetActive(false);
        trigger.SetActive(false);
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        curQuest = tracker.dialogues[tracker.dialogueProg];
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pickedUp && !textbox.activated){
            trigger.SetActive(true);
        }

        if(curQuest.subject == subject && !pickedUp){
            marker.SetActive(true);
        }
        else{
            marker.SetActive(false);
        }

        //if player presses space and textbox not currently active, choose either quest or idle text and activate textbox
        if(entered && Input.GetButtonDown("Use") && !textbox.activated){
            
            //
            if(!pickedUp && curQuest.subject == subject){
                pickedUp = true;
                if(tracker.hasFood){
                    if(curQuest.satisfiesQuest(tracker.foodObject.texture, tracker.foodObject.warmth, tracker.foodObject.flavor)){
                        textbox.SetDialogue(curQuest.bestEnding);
                        tracker.dialogueProg++;
                    }
                    else{
                        textbox.SetDialogue(curQuest.goodEnding);
                    }
                }
                else{
                    textbox.SetDialogue(curQuest.dialogueSegments);
                }
                
            }
            else{
                //chooses whether idle text is sequential or random
                //sequential text resets once end of array has been reached
                switch(subject){
                    case "Camellia":
                    idlePos = (idlePos + 1)%idleText.Length;
                    break;

                    default:
                    idlePos = (int)Random.Range(0, idleText.Length);
                    break;
                }
                TextAsset temp = idleText[idlePos];
                textbox.SetDialogue(JsonUtility.FromJson<Dialogue>(temp.text).dialogueSegments);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other){
        
        entered = true;
    }

    void OnTriggerExit2D(Collider2D other){
        // marker.SetActive(false);
        entered = false;
    }
}
