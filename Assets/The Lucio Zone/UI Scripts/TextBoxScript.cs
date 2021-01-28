using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxScript : MonoBehaviour
{

    #region Public Variables
    [Header("In Scene Object References")]
    [Tooltip("GameObject that contains the textbox. This should include any art part of the textbox. Needed for hierarchy manipulation.")]
    public Image textboxImage;
    [Tooltip("Text you want to be displayed")]
    public Text textbox;

    [Tooltip("Text for the name of the speaker")]
    public Text nameText;

    [Tooltip("Input file for dialogue. Should be written in json style")]
    public TextAsset textFile;

    [Tooltip("Name and image of character. Name of character must exactly match name given in textFile")]
    public List<CharacterData> characterInformation = new List<CharacterData>();

    [Tooltip("Seconds between adding another letter")]
    public float scrollSpeed = 0.0625f;
    
    #endregion

    #region Private Variables
    private int letter = 0; //keeps track of letters added to text
    private int loops = 0; //tracks where program is in the dialogue
    private bool speedUp = false; //tracks if conversation has been sped up;
    private string message; //text part of the dialogue currently being shown
    private float timer = 1f; //counts when the next letter should be added
    private string currentSpeaker; //who is currently speaking in dialogue, determined with speaker array
    private Dialogue dialogue;
    private bool activated;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dialogue = JsonUtility.FromJson<Dialogue>(textFile.text);        
        textbox.text = "";

        //deactivate all of the visual elements
        DeactivateObjects();
        
        //setting up variables for first part of dialogue
        message = dialogue.dialogueSegments[loops].text;
        currentSpeaker = dialogue.dialogueSegments[loops].speaker;
        loops ++;
        ChangeName();
    }

    // Update is called once per frame
    void Update()
    {
        //if there are letters to add and required amount of time has passed
        if(Time.time > timer && letter < message.Length && activated){
            AddLetter();
        }

        //if user presses "x" text will speed up or go to next part of dialogue
        if(Input.GetKeyDown(KeyCode.X)){
            SpeedUp();
        }

        //runs through given list of speaker images, darkens all non current speakers
        foreach(CharacterData data in characterInformation){
            if(data.name == currentSpeaker){
                LightenImage(data.image);
            }
            else{
                DarkenImage(data.image);
            }
        }
    }

    //speed up or move to next message
    void SpeedUp(){

        //speed up scrolling rate
        if(!speedUp && activated){
            scrollSpeed /= 9;
            speedUp = true;
        }

        if(!activated){
            ActivateObjects();
        }

        //move to next message in dialogue if end of message has been reached
        if(letter >= message.Length && loops < dialogue.dialogueSegments.Length){
            //set unique variables up for next message
            message = dialogue.dialogueSegments[loops].text;
            currentSpeaker = dialogue.dialogueSegments[loops].speaker;
            ChangeName();
            loops++;

            //reset general variables
            speedUp = false;
            scrollSpeed *= 9;
            letter = 0;
            textbox.text = "";
        }

        //if the end of the dialogue has been reached add progress to NarrativeTracker and end dialogue
        if(loops >= dialogue.dialogueSegments.Length && letter >= message.Length){
            DeactivateObjects();
        }
        
    }

    //add letter from current message to textbox 
    //increment letter counter and set next time to add letter
    void AddLetter(){
        textbox.text += message[letter];
        letter++;
        timer = Time.time + scrollSpeed;
    }

    //darkens input image and decreases size
    void DarkenImage(Image target){
        Color32 temp = target.color;

        if(temp.r > 60){
            temp.r -= 1;
            temp.g -= 1;
            temp.b -= 1;

            target.transform.localScale -= new Vector3(0.0005f * target.transform.localScale.x, 0.0005f * target.transform.localScale.y, 0);
        }
        
        target.color = temp;
    }


    //lightens input image and increases size
    void LightenImage(Image target){
        Color32 temp = target.color;
        
        if(temp.r < 255){
            temp.r += 1;
            temp.g += 1;
            temp.b += 1;

            target.transform.localScale += new Vector3(0.0005f * target.transform.localScale.x, 0.0005f * target.transform.localScale.y, 0);
        }
        target.transform.SetAsLastSibling();
        textboxImage.transform.parent.SetAsLastSibling(); 
               
        target.color = temp;
    }

    //Changes name displayed
    void ChangeName(){
        nameText.text = currentSpeaker;
    }

    //Activates all of the visual elements
    //Basically let's program know it should start to display
    void ActivateObjects(){
        textbox.gameObject.SetActive(true);
        foreach(CharacterData data in characterInformation){
            data.image.gameObject.SetActive(true);
        }
        nameText.gameObject.SetActive(true);
        textboxImage.gameObject.SetActive(true);

        activated = true;
    }

    //Turns all visual elements inactive and prevents program from progressing
    void DeactivateObjects(){
        textbox.gameObject.SetActive(false);
        foreach(CharacterData data in characterInformation){
            data.image.gameObject.SetActive(false);
        }
        nameText.gameObject.SetActive(false);
        textboxImage.gameObject.SetActive(false);

        activated = false;
    }

}

[System.Serializable]
//Relevant display data. Has fields for name of the character and the in-scene image of them.
public class CharacterData
{
    public string name;
    public Image image;
    
}

[System.Serializable]
//Represents a portion of the dialogue. Has fields for text to be displayed and the name of the speaker
public class dialogueSegment
{
   public string text;
   public string speaker;
}

[System.Serializable]
//Represents an entire dialogue. The encompassing object for the json file
public class Dialogue
{
    public dialogueSegment[] dialogueSegments;
   // public string progression;
    //public string target;
}