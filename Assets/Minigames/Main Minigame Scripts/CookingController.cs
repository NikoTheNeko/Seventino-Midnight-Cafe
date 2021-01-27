using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingController : MonoBehaviour
{
    #region public variables

    [Header("Minigame Objects")]
    [Tooltip("This is where all of the minigames are stored.")]
    public GameObject[] Minigames;

    [Header("Minigame Camera Locations")]
    [Tooltip("This is where all of the CAMERA locations are stored so that the camera cam move to each location")]
    public Transform[] CameraLocations;

    [Header("Camera and other Game Objects")]
    [Tooltip("The Main Camera that is being used")]
    public Transform Camera;

    [Tooltip("The speed at which the camera moved")]
    public float CameraSpeed = 0.125f;
    
    [Tooltip("The button to progress through each track")]
    public Button NextButton;

    #endregion

    #region Private Variables

    //Minigame Number keeps track of the minigames that go through each thing.
    private int MinigameNumber = 0;

    //This variable checks to send the activation messages so it's not constantly being called
    private bool ActivationMessageSent = false;

    #endregion

    // Update is called once per frame
    void FixedUpdate(){
        UpdateMinigames();
    }

    #region Minigame and Camera Controls

    /**
        UpdateMinigames holds all of the functions to run everything
        This is basically so it's all packed in one neat function that isn't
        update for easy debugging.
    **/
    public void UpdateMinigames(){
        ActivateMinigame();
        MoveCamera();
    }

    #region The actual functions for Minigame Control

    /**
        Move Camera uses lerp to smoothly move the camera using
        the CameraSpeed variable
    **/
    private void MoveCamera(){
        Vector3 NewLocation = Vector3.Lerp(Camera.position, 
                                            CameraLocations[MinigameNumber].position,
                                            CameraSpeed);
        Camera.position = NewLocation;
    }

    /**
        This sends a message to the minigames tho turn on their minigames
    **/
    private void ActivateMinigame(){
        if(ActivationMessageSent == false){
            Minigames[MinigameNumber].SendMessage("ActivateMinigame");
            ActivationMessageSent = true;
        }
    }

    #endregion
    #endregion

    #region Interaction with Other Objects

    /**
        MinigameFinished is called by the minigame to notify when the game is finished
        This allows the Bookmark Button to become clickable and interactable allowing
        the game to progress. This may be called by some minigames at the start
        as they are optional to complete
    **/    
    public void MinigameFinished(){
        NextButton.interactable = true;
    }

    /**
        This funciton gets called on by the minigames to notify
        that the minigame is completed
    **/
    public void IncrementMinigame(){
        Minigames[MinigameNumber].SendMessage("DeactivateMinigame");
        if(MinigameNumber + 1 < Minigames.Length)
            MinigameNumber++;
        ActivationMessageSent = false;
    }

    #endregion

}
