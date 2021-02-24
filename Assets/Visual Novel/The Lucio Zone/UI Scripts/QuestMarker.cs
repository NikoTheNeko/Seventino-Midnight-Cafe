using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMarker : MonoBehaviour
{
    public GameObject marker;
    public TextBoxScript textbox;
    public GameObject trigger;

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
        Debug.Log("activated = " + textbox.activated);
        Debug.Log("pickedUp = " + pickedUp);
        if(pickedUp && !textbox.activated){
            Debug.Log("triggered");
            trigger.SetActive(true);
        }
    }


    void OnTriggerEnter2D(Collider2D other){
        marker.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other){
        marker.SetActive(false);
    }
    
    void OnTriggerStay2D(Collider2D other){
        Debug.Log("colliding");
        if(Input.GetButton("Use")){
            // Time.timeScale = 0f;
            Debug.Log("entered the Lucio Zone");
            textbox.ActivateObjects();
            pickedUp = true;
        }
    }
}
