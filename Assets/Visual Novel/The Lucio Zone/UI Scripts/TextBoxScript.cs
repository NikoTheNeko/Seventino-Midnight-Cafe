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
    public TextAsset[] questText;
    public TextAsset[] idleText;

    [Tooltip("Name and image of character. Name of character must exactly match name given in textFile")]
    public List<CharacterData> characterInformation = new List<CharacterData>(); //note to self put emotions in characterdata

    //public CharacterExpressions[] expressions;
    //public CharacterExpressions chefExpressions;

    [Tooltip("Seconds between adding another letter")]
    public float scrollSpeed = 0.0625f;
    public bool activated;
    #endregion

    #region Private Variables
    private int letter = 0; //keeps track of letters added to text
    private int loops = 0; //tracks where program is in the dialogue
    private bool speedUp = false; //tracks if conversation has been sped up;
    private string message; //text part of the dialogue currently being shown
    private float timer = 1f; //counts when the next letter should be added
    private string currentSpeaker; //who is currently speaking in dialogue, determined with speaker array
    private string emotion; //holds the emotion of the current speaker in a non-philisophical type way
    private Dialogue dialogue;
    
    private int curDialogue = 0; //temp variable for counting which dialogue is being looked at. recommend putting on inventory for persistent data

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dialogue = JsonUtility.FromJson<Dialogue>(questText[curDialogue].text);     
        textbox.text = "";
        curDialogue++;

        //deactivate all of the visual elements
        DeactivateObjects();
        
        //setting up variables for first part of dialogue
        message = dialogue.dialogueSegments[loops].text;
        currentSpeaker = dialogue.dialogueSegments[loops].speaker;
        emotion = dialogue.dialogueSegments[loops].emotion;
        loops ++;
        ChangeName();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("CurDialog = " + curDialogue);
        //if there are letters to add and required amount of time has passed
        if(Time.time > timer && letter < message.Length && activated){
            AddLetter();
        }

        //if user presses "x" text will speed up or go to next part of dialogue
        if(Input.GetButtonDown("Use")){
            SpeedUp();
        }

        //runs through given list of speaker images, darkens all non current speakers
        foreach(CharacterData data in characterInformation){
            if(data.name == currentSpeaker){
                Debug.Log(emotion);
                Sprite temp = data.getEmotion(emotion);
                LightenImage(data.image, temp);
            }
            else{
                DarkenImage(data.image);
            }
        }
    }

    //method called when Use is pressed
    //has variety of effects based on context
    void SpeedUp(){

        //speed up scrolling rate
        if(!speedUp && activated){
            scrollSpeed /= 9;
            speedUp = true;
        }

        //move to next message in dialogue if end of message has been reached
        if(letter >= message.Length && loops < dialogue.dialogueSegments.Length){
            //set unique variables up for next message
            message = dialogue.dialogueSegments[loops].text;
            currentSpeaker = dialogue.dialogueSegments[loops].speaker;
            emotion = dialogue.dialogueSegments[loops].emotion;
            ChangeName();
            loops++;

            //reset general variables
            speedUp = false;
            scrollSpeed = 0.0625f;
            letter = 0;
            textbox.text = "";
        }

        //if end of final dialogue has been reached
        if(curDialogue >= questText.Length && loops >= dialogue.dialogueSegments.Length && letter >= message.Length){
            Debug.Log("called it");
            DeactivateObjects();
        }

        //if the end of the dialogue has been reached add progress to NarrativeTracker and end dialogue
        if(loops >= dialogue.dialogueSegments.Length && letter >= message.Length && curDialogue < questText.Length){
            DeactivateObjects();
            dialogue = JsonUtility.FromJson<Dialogue>(questText[curDialogue].text);
            curDialogue++;


            loops = 0;
            letter = 0;
            speedUp = false;
            textbox.text = "";
            scrollSpeed = 0.0625f;
            message = dialogue.dialogueSegments[loops].text;
            currentSpeaker = dialogue.dialogueSegments[loops].speaker;
            emotion = dialogue.dialogueSegments[loops].emotion;
            ChangeName();
            loops++;
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
            temp.r -= 10;
            temp.g -= 10;
            temp.b -= 10;

            target.transform.localScale -= new Vector3(0.0005f * target.transform.localScale.x, 0.0005f * target.transform.localScale.y, 0);
        }
        
        target.color = temp;
    }


    //lightens input image and increases size
    void LightenImage(Image target, Sprite emotion){
        Color32 temp = target.color;
        //CharacterEmotion emotion = expressions[speaker][]
        target.sprite = emotion;
        
        if(temp.r < 255){
            temp.r += 10;
            temp.g += 10;
            temp.b += 10;

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
    public void ActivateObjects(){
        textbox.gameObject.SetActive(true);
        foreach(CharacterData data in characterInformation){
            data.image.gameObject.SetActive(true);
        }
        nameText.gameObject.SetActive(true);
        textboxImage.gameObject.SetActive(true);

        activated = true;
    }

    //Turns all visual elements inactive and prevents program from progressing
    public void DeactivateObjects(){
        textbox.gameObject.SetActive(false);
        foreach(CharacterData data in characterInformation){
            data.image.gameObject.SetActive(false);
        }
        nameText.gameObject.SetActive(false);
        textboxImage.gameObject.SetActive(false);

        activated = false;
    }

    public void SetDialogue(TextAsset text){
        dialogue = JsonUtility.FromJson<Dialogue>(text.text);
        loops = 0;
        letter = 0;
        speedUp = false;
        textbox.text = "";
        scrollSpeed = 0.0625f;
        message = dialogue.dialogueSegments[loops].text;
        currentSpeaker = dialogue.dialogueSegments[loops].speaker;
        emotion = dialogue.dialogueSegments[loops].emotion;
    }
}


[System.Serializable]
//Relevant display data. Has fields for name of the character and the in-scene image of them.
public class CharacterData
{
    public string name;
    public Image image;
    public CharacterEmotion[] emotions;

    public Sprite getEmotion(string name){
        foreach(CharacterEmotion emotion in emotions){
            if (emotion.emotion == name){
                return emotion.image;
            }
        }
        return emotions[0].image;
    }

    //emotion for a character
    //identified using emotion
    [System.Serializable]
    public class CharacterEmotion
    {
        public string emotion;
        public Sprite image;
    }
}

[System.Serializable]
//Represents a portion of the dialogue. Has fields for text to be displayed and the name of the speaker
public class dialogueSegment
{
   public string text;
   public string speaker;
   public string emotion;
}

[System.Serializable]
//Represents an entire dialogue. The encompassing object for the json file
public class Dialogue
{
    public dialogueSegment[] dialogueSegments;
    // public string progression;
    //public string target;
}