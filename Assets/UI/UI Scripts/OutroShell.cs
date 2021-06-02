using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroShell : MonoBehaviour
{
    public TextBoxScript textBox;
    public TextAsset introText;
    public TextAsset outroText;
    public SceneChange sceneChange;
    private Dialogue dialogue;
    private InventoryTracker tracker;
    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        if(tracker.dialogueProg > 0){
            dialogue = JsonUtility.FromJson<Dialogue>(outroText.text);
        }
        else{
            dialogue = JsonUtility.FromJson<Dialogue>(introText.text);
        }
        new WaitForSeconds(1);
        textBox.SetDialogue(dialogue.dialogueSegments);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Use")){
            textBox.SpeedUp();
        }

        if(!textBox.activated){
            if(tracker.dialogueProg > 0){
                sceneChange.SceneChangeTo("TitleScreen");
            }
            else{
                sceneChange.SceneChangeTo("Cafe Walk Around");
            }
        }
    }
}
