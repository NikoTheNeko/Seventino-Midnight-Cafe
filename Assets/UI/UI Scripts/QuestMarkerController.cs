using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarkerController : MonoBehaviour
{
    public GameObject[] QuestMarkers;
    private InventoryTracker tracker;
    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        for(int i = tracker.dialogueProg + 1; i < QuestMarkers.Length; i++){
            QuestMarkers[i].SetActive(false);
        }
    }

    public void AdvanceProgression(){
        tracker.dialogueProg++;
        QuestMarkers[tracker.dialogueProg].SetActive(true);
    }
}
