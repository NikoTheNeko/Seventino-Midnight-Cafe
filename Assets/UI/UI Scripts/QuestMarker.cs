using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMarker : MonoBehaviour
{
    public GameObject marker;
    public TextBoxScript textbox;
    public GameObject trigger;
    public GameObject successCG;
    public QuestMarkerController controller;
    bool entered = false;
    public TextAsset[] idleText;
    public GameObject talkTo;
    InventoryTracker tracker;
    Dialogue curQuest;
    public string subject;
    private int idlePos = -1;

    public bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        successCG.SetActive(false);
        marker.SetActive(false);
        trigger.SetActive(false);
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        talkTo.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        curQuest = tracker.dialogues[tracker.dialogueProg];

        if(pickedUp && !textbox.activated){
            trigger.SetActive(true);
        }

        if(curQuest.subject == subject && !pickedUp){
            marker.SetActive(true);
        }
        else{
            marker.SetActive(false);
        }

        if(entered && Input.GetKeyDown(KeyCode.Space) && Time.timeScale > 0f){
                //if player presses space and textbox not currently active, choose either quest or idle text and activates textbox
            if(!textbox.activated){
                //if NPC is the target of current quest
                if(!pickedUp && curQuest.subject == subject){
                    pickedUp = true;
                    //if player has a dish, choose an ending
                    if(tracker.hasFood){
                        tracker.hasFood = false;
                        //if food being carried satisfies quest, give good ending and advance dialogue progression
                        if(curQuest.satisfiesQuest(tracker.texture, tracker.warmth, tracker.flavor)){
                            StartCoroutine(advanceProgression());
                        }
                        else{
                            textbox.SetDialogue(curQuest.goodEnding);
                        }
                    }
                    else{
                        //give player quest
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
            else{
                textbox.SpeedUp();
            }
        }
        
        
    }

    IEnumerator advanceProgression(){
        Debug.Log("quest succeeded");
        successCG.SetActive(true);
        textbox.activated = true;
        yield return new WaitForSeconds(2);
        textbox.SetDialogue(curQuest.bestEnding);
        // textbox.SpeedUp();
        controller.AdvanceProgression();
    }


    void OnTriggerEnter2D(Collider2D other){
        
        entered = true;
        talkTo.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other){
        entered = false;
        talkTo.SetActive(false);
    }
}
