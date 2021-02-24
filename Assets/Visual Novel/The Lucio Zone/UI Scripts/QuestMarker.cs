using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMarker : MonoBehaviour
{
    public GameObject marker;
    public TextBoxScript textbox;

    private bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        marker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D other){
        marker.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other){
        marker.SetActive(false);
    }
    
    void OnTriggerStay2D(Collider2D other){
        if(Input.GetButton("Use") && !pickedUp && !textbox.activated){
            // Time.timeScale = 0f;
            textbox.ActivateObjects();
            pickedUp = !pickedUp;
        }
    }
}
