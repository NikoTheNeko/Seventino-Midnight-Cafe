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

    private bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        marker.SetActive(false);
        trigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(pickedUp && !textbox.activated){
            trigger.SetActive(true);
        }
        Debug.Log("activated: " + textbox.activated);

        if(entered && Input.GetButtonDown("Use") && !textbox.activated){
            
            if(!pickedUp){
                pickedUp = true;
                textbox.SetDialogue(questText);
            }
            else if(pickedUp){
                TextAsset temp = idleText[(int)Random.Range(0, idleText.Length)];
                Debug.Log(temp.text);
                textbox.SetDialogue(temp);
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
    
    // void OnTriggerStay2D(Collider2D other){
    //     Debug.Log("colliding");
    //     if(Input.GetButton("Use")){
    //         // Time.timeScale = 0f;
    //         Debug.Log("entered the Lucio Zone");
    //         textbox.ActivateObjects();
    //         pickedUp = true;
    //     }
    // }
}
