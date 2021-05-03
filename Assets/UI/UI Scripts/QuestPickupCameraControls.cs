using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPickupCameraControls : MonoBehaviour{

    public Transform Camera;
    public Transform[] CameraLocations;
    public float CameraSpeed = 0.5f;

    private int CurrentLocation = 0;

    // Update is called once per frame
    void Update(){
        Vector3 CameraTarget = CameraLocations[CurrentLocation].position;
        Vector3 NewLoc = Vector3.Lerp(Camera.position, CameraTarget, CameraSpeed);
        Camera.position = NewLoc;
    }

    /**
        0 - Quest Pick Up
        1 - VN
        2 - Cookbook
    **/
    public void SetLocation(int NewLocation){
        if(NewLocation >= 0 && NewLocation <= 2)
            CurrentLocation = NewLocation;
    }

}
