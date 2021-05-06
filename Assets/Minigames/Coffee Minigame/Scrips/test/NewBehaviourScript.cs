using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour{
    public GameObject self;
    public Slider sliderTest;

    public float TiltAngle = 45;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        float newAngle = sliderTest.value * TiltAngle;

        self.transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);

    }
}
