using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryOverStats : MonoBehaviour
{

    private InventoryTracker tracker;


    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();    
    }

    public void CarryStatsPlease(){
        tracker.CarryCoffeeStats();
    }

    public void RemoveBean(int BeanSpot){
        tracker.remove(BeanSpot);
    }

}
