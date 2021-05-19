using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalBars : MonoBehaviour{

    private InventoryTracker tracker;

    private float TextureMin = 10f;
    private float TextureMax = 10f;
    private float WarmthMin = 10f;
    private float WarmthMax = 10f;
    private float FlavorMin = 10f;
    private float FlavorMax = 10f;

    public Slider TextureMinBar;
    public Slider TextureMaxBar;
    public Slider WarmthMinBar;
    public Slider WarmthMaxBar;
    public Slider FlavorMinBar;
    public Slider FlavorMaxBar;

    // Start is called before the first frame update
    void Start(){
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        GetGoals();
    }

    // Update is called once per frame
    void Update(){
        SetGoals();
    }

    /**
        This gets the goals from the parameters in the quest thing
        thank you JSON Derulo
    **/
    private void GetGoals(){
        float[] temp = tracker.getCurrentQuestParams();
        TextureMin = temp[0];
        TextureMax = temp[1];
        WarmthMin = temp[2];
        WarmthMax = temp[3];
        FlavorMin = temp[4];
        FlavorMax = temp[5];
    }

    /**
        This sets the goal bars both min and max
        The max is a bit finnicky, so it's always 100 - the max
        that way it can actually be like the correct way
    **/
    private void SetGoals(){
        TextureMinBar.value = TextureMin;
        TextureMaxBar.value = 100 - TextureMax; 
        WarmthMinBar.value = WarmthMin;
        WarmthMaxBar.value = 100 - WarmthMax; 
        FlavorMinBar.value = FlavorMin;
        FlavorMaxBar.value = 100 - FlavorMax; 

        if(TextureMinBar.value < 10)
            TextureMinBar.value = 10f;
        if(TextureMaxBar.value < 10)
            TextureMaxBar.value = 10f;

        if(WarmthMinBar.value < 10)
            WarmthMinBar.value = 10f;
        if(WarmthMaxBar.value < 10)
            WarmthMaxBar.value = 10f;

        if(FlavorMinBar.value < 10)
            FlavorMinBar.value = 10f;
        if(FlavorMaxBar.value < 10)
            FlavorMaxBar.value = 10f;

    }


}
