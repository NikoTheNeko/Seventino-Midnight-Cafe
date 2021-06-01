using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroShell : MonoBehaviour
{
    public TextBoxScript textBox;
    public TextAsset dialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Use")){
            if(!textBox.activated){
                textBox.SetDialogue(JsonUtility.FromJson<Dialogue>(dialogue.text).dialogueSegments);
            }
            else{
                textBox.SpeedUp();
            }
        }
    }
}
