using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeShell : MonoBehaviour
{

    public SceneChange change;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            change.SceneChangeTo("Quest Selection Screen");
        }
    }
}
