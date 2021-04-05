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
        
    }

    // Update is called once per frame
    void Update()
    {
        curDialogue = tracker.dialogues[tracker.dialogueProg];
        if(pickedUp && !textbox.activated){
            trigger.SetActive(true);
        }
        Debug.Log("activated: " + textbox.activated);

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
