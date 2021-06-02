using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeShell : MonoBehaviour
{

    public SceneChange change;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            InventoryTracker tracker = GameObject.FindWithTag("InventoryTracker").GetComponent<InventoryTracker>();
            if(tracker.dialogueProg >= 4){
                change.SceneChangeTo("OutroScene");
            }
            else{
                change.SceneChangeTo("Quest Selection Screen");
            }
            
        }
    }
}
