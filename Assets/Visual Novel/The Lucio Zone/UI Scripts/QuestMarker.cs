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
    public TextAsset questText;
    public TextAsset[] idleText;
    InventoryTracker tracker;
    Dialogue curDialogue;
    public string subject;

    private bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
        marker.SetActive(false);
        trigger.SetActive(false);
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        curDialogue = tracker.dialogues[tracker.dialogueProg];
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pickedUp && !textbox.activated){
            trigger.SetActive(true);
        }

        if(entered && Input.GetButtonDown("Use") && !textbox.activated){
            
            
            if(!pickedUp && curDialogue.subject == subject){
                pickedUp = true;
                textbox.SetDialogue(curDialogue);
            }
            else{
                TextAsset temp = idleText[(int)Random.Range(0, idleText.Length)];
                textbox.SetDialogue(JsonUtility.FromJson<Dialogue>(temp.text));
            }
            
        }
    }


    void OnTriggerEnter2D(Collider2D other){
        marker.SetActive(true);
        entered = true;
    }

    void OnTriggerExit2D(Collider2D other){
        marker.SetActive(false);
        entered = false;
    }
}
