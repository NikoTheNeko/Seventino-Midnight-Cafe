using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingController : MonoBehaviour
{
    #region public variables

    [Header("Minigame Objects")]
    [Tooltip("This is the number of minigames")]
    public int AmountOfMinigames = 1;
    [Tooltip("This is where all of the minigames are stored.")]
    public GameObject[] Minigames;

    [Header("Minigame Camera Locations")]
    [Tooltip("This is where all of the CAMERA locations are stored so that the camera cam move to each location")]
    public Transform[] CameraLocations;

    [Tooltip("This holds all the buttons for the minigames")]
    public Button[] Choices;

    [Header("Camera and other Game Objects")]
    [Tooltip("The Main Camera that is being used")]
    public Transform Camera;

    [Tooltip("The speed at which the camera moved")]
    public float CameraSpeed = 0.125f;
    
    [Tooltip("The button to progress through each track")]
    public Button NextButton;

    [Tooltip("The Book that moves around")]
    public Transform Book;
    [Tooltip("Left Anchor for the book")]
    public Transform LeftAnchor;
    [Tooltip("Center Anchor for the book")]
    public Transform CenterAnchor;

    [Header("Canvases")]
    public GameObject Tutorial;
    public GameObject BeanPicking;
    public GameObject Instructions;
    public GameObject ChoiceButtons;

    #endregion

    #region Private Variables

    //Minigame Number keeps track of the minigames that go through each thing.
    public int MinigameNumber = -3;

    //This variable checks to send the activation messages so it's not constantly being called
    private bool ActivationMessageSent = false;

    //This is the state of the game 0 = Grind, 1 = Brew Pour, 2 = Addon
    private int GameState = 0;

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
        BookControl();
        MoveCamera();
    }

    #region The actual functions for Minigame Control

    /**
        Move Camera uses lerp to smoothly move the camera using
        the CameraSpeed variable
    **/
    private void MoveCamera(){
        if(MinigameNumber < 0)
            return;
        Vector3 NewLocation = Vector3.Lerp(Camera.position, 
                                            CameraLocations[MinigameNumber].position,
                                            CameraSpeed);
        Camera.position = NewLocation;
    }

    private void MoveBook(Transform Anchor){
        Vector3 NewLocation = Vector3.Lerp(Book.position, 
                                            Anchor.position,
                                            CameraSpeed);
        Book.position = NewLocation;
    }

    private void BookControl(){
        if(MinigameNumber == -3){
            MoveBook(CenterAnchor);
            Tutorial.SetActive(true);
            BeanPicking.SetActive(false);
            ChoiceButtons.SetActive(false);
            Instructions.SetActive(false);
        } else if(MinigameNumber == -2){
            MoveBook(CenterAnchor);
            Tutorial.SetActive(false);
            BeanPicking.SetActive(true);
            ChoiceButtons.SetActive(false);
            Instructions.SetActive(false);
        } else if(MinigameNumber == -1){
            MoveBook(CenterAnchor);
            Tutorial.SetActive(false);
            BeanPicking.SetActive(false);
            ChoiceButtons.SetActive(true);
            Instructions.SetActive(false);
        } else {
            Tutorial.SetActive(false);
            BeanPicking.SetActive(false);
            ChoiceButtons.SetActive(false);
            Instructions.SetActive(true);
            MoveBook(LeftAnchor);
        }

        switch(GameState){
            case 0:
            Choices[0].interactable = true;
            break;

            case 1:
            Choices[0].interactable = false;
            Choices[1].interactable = true;
            break;

            case 2:
            Choices[1].interactable = false;
                for(int i = 2; i < Choices.Length; i++){
                    Choices[i].interactable = true;
                }
            break;
        }
    }

    /**
        This sends a message to the minigames tho turn on their minigames
    **/
    private void ActivateMinigame(){
        if(ActivationMessageSent == false && MinigameNumber >= 0){
            MoveCamera();
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
    public void MinigameFinished(int MinigameNumber, bool CanAccess){
        NextButton.interactable = true;
        if(MinigameNumber == 0 && GameState == 0)
            GameState = 1;
        if(MinigameNumber == 1 && GameState == 1)
            GameState = 2;
    }

    /**
        This funciton gets called on by the minigames to notify
        that the minigame is completed
    **/
    public void GoToMinigame(int NewMinigameNumber){
        if(MinigameNumber != -1)
            Minigames[MinigameNumber].SendMessage("DeactivateMinigame");
        if(NewMinigameNumber < Minigames.Length){
            ActivationMessageSent = false;
            MinigameNumber = NewMinigameNumber;
        }
    }

    public void ShowChoices(){
        MinigameNumber = -1;
        Instructions.SetActive(false);
        ChoiceButtons.SetActive(true);
    }

    public void StartCooking(){
        MinigameNumber = -2;
    }

    public void BeanSelected(){
        MinigameNumber = -1;
    }

    #endregion

}
