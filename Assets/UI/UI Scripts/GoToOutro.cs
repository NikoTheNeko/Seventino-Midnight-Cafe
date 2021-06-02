using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToOutro : MonoBehaviour
{
    public SceneChange sceneChange;
    public TextBoxScript textbox;
    private InventoryTracker tracker;
    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!textbox.activated && tracker.dialogueProg >= 4){
            sceneChange.SceneChangeTo("OutroScene");
        }
    }
}
