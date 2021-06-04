using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class TextBoxScript : MonoBehaviour
{

    #region Public Variables
    [Header("In Scene Object References")]
    [Tooltip("GameObject that contains the textbox. This should include any art part of the textbox. Needed for hierarchy manipulation.")]
    public Image textboxImage;
    [Tooltip("Text you want to be displayed")]
    public TextMeshProUGUI textbox;

    [Tooltip("Text for the name of the speaker")]
    public TextMeshProUGUI nameText;
    public GameObject namePlate;

    [Tooltip("Name and image of character. Name of character must exactly match name given in JSON file")]
    public List<CharacterData> characterInformation = new List<CharacterData>(); //note to self put emotions in characterdata

    public AudioSource audio;
    public AudioMixerGroup mixer;
    public AudioClip CustomerSoundClip;
    public AudioClip ChefSoundClip;

    [Tooltip("Seconds between adding another letter")]
    public float scrollSpeed = 0.03125f;
    public bool activated = false;
    public GameObject successCG;
    public GameObject chefAnchor;
    public GameObject customerAnchor;
    public GameObject goNext;
    #endregion

    #region Private Variables
    private int letter = 0; //keeps track of letters added to text
    private int loops = 0; //tracks where program is in the dialogue
    private bool speedUp = false; //tracks if conversation has been sped up;
    private string message; //text part of the dialogue currently being shown
    private float timer = 1f; //counts when the next letter should be added
    private string currentSpeaker; //who is currently speaking in dialogue, determined with speaker array
    private string emotion; //holds the emotion of the current speaker in a non-philisophical type way
    private dialogueSegment[] dialogueSegments;
    private InventoryTracker tracker;
    

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        audio.outputAudioMixerGroup = mixer;
        textbox.text = "";
        // tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        //deactivate all of the visual elements
        DeactivateObjects();
                
    }

    // Update is called once per frame
    void Update()
    {
        if(activated){
            //if there are letters to add and required amount of time has passed
            if(Time.time > timer && letter < message.Length){
                AddLetter();
            }

            //runs through given list of speaker images, darkens all non current speakers
            foreach(CharacterData data in characterInformation){
                if(currentSpeaker.Contains(data.name)){
                    Debug.Log(emotion);
                    Sprite temp = data.getEmotion(emotion);
                    LightenImage(data.image, temp);
                }
                else{
                    DarkenImage(data.image);
                }
            }

            if(letter >= message.Length){
                goNext.SetActive(true);
            }
            else{
                goNext.SetActive(false);
            }
        }
    }

    //method called when Use is pressed
    //has variety of effects based on context
    public void SpeedUp(){
        //move to next message in dialogue if end of message has been reached
        if(letter >= message.Length && loops < dialogueSegments.Length - 1){
            //set unique variables up for next message
            loops++;
            message = dialogueSegments[loops].text;
            currentSpeaker = dialogueSegments[loops].speaker;
            emotion = dialogueSegments[loops].emotion;
            ChangeName();
            
            //reset general variables
            speedUp = false;
            scrollSpeed = 0.0625f;
            letter = 0;
            textbox.text = "";
        }

        //if end of dialogue has been reached
        else if(loops >= dialogueSegments.Length - 1 && letter >= message.Length){
            DeactivateObjects();
        }

        //speed up scrolling rate
        else if(!speedUp && activated){
            textbox.text = message;
            letter = message.Length;
            // scrollSpeed /= 20;
            speedUp = true;
        }
        
    }

    //add letter from current message to textbox 
    //increment letter counter and set next time to add letter
    void AddLetter(){
        string buffer = "";
        if(message[letter] == '<'){
            while(message[letter] != '>'){
                buffer += message[letter];
                letter++;
            }
        }
        textbox.text += buffer;
        textbox.text += message[letter];
        if(currentSpeaker.Contains("Chef")){
            audio.PlayOneShot(ChefSoundClip);
        }
        else{
            audio.PlayOneShot(CustomerSoundClip);
        }
           
        
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

            target.transform.localScale -= new Vector3(0.005f * target.transform.localScale.x, 0.005f * target.transform.localScale.y, 0);
        }
        
        target.color = temp;
    }


    //lightens input image and increases size
    //also changes sprite to input emotion
    void LightenImage(Image target, Sprite emotion){
        Color32 temp = target.color;
        target.sprite = emotion;
        
        if(temp.r < 255){
            temp.r += 10;
            temp.g += 10;
            temp.b += 10;

            target.transform.localScale += new Vector3(0.005f * target.transform.localScale.x, 0.005f * target.transform.localScale.y, 0);
        }
        target.transform.SetSiblingIndex(2);
        textboxImage.transform.parent.SetAsLastSibling(); 
               
        target.color = temp;
    }

    //Changes name displayed
    void ChangeName(){
        nameText.text = currentSpeaker;
        if(currentSpeaker.Contains("Chef")){
            namePlate.transform.position = chefAnchor.transform.position;
        }
        else{
            namePlate.transform.position = customerAnchor.transform.position;
        }
    }

    //Activates all of the visual elements
    //Basically let's program know it should start to display
    public void ActivateObjects(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            player.GetComponent<PlayerCombatTesting>().CanMove = false;
        }
        

        textbox.gameObject.SetActive(true);
        namePlate.SetActive(true);
        foreach(CharacterData data in characterInformation){
            data.image.gameObject.SetActive(true);
        }
        nameText.gameObject.SetActive(true);
        textboxImage.gameObject.SetActive(true);

        activated = true;
    }

    //Turns all visual elements inactive and prevents program from progressing
    public void DeactivateObjects(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            player.GetComponent<PlayerCombatTesting>().CanMove = true;
        }

        textbox.gameObject.SetActive(false);
        namePlate.SetActive(false);
        goNext.SetActive(false);
        foreach(CharacterData data in characterInformation){
            data.image.color = new Color32(55, 55, 55, 255);
            data.image.transform.localScale = new Vector3(0.09828957f,0.09828957f,0.09828957f);
            data.image.gameObject.SetActive(false);
        }
        nameText.gameObject.SetActive(false);
        textboxImage.gameObject.SetActive(false);
        successCG.SetActive(false);

        activated = false;

        // if(tracker.dialogueProg >= 4){
            // SceneManager.LoadScene("OutroScene", LoadSceneMode.Single);
            // Debug.Log("going to new scene");
        // }
    }

    //set dialogue to given TextAsset, resets variables
    //then activates self
    public void SetDialogue(dialogueSegment[] text){
        dialogueSegments = text;
        loops = 0;
        letter = 0;
        speedUp = false;
        textbox.text = "";
        scrollSpeed = 0.0625f;
        message = dialogueSegments[loops].text;
        currentSpeaker = dialogueSegments[loops].speaker;
        emotion = dialogueSegments[loops].emotion;
        ChangeName();
        ActivateObjects();
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
    public string subject;
    public dialogueSegment[] dialogueSegments;
    public dialogueSegment[] goodEnding;
    public dialogueSegment[] bestEnding;
    public float textureMin;
    public float textureMax;
    public float warmthMin;
    public float warmthMax;
    public float flavorMin;
    public float flavorMax;

    public bool satisfiesQuest(float texture, float warmth, float flavor){
        return texture >= textureMin && texture <= textureMax 
                && warmth >= warmthMin && warmth <= warmthMax 
                && flavor >= flavorMin && flavor <= flavorMax;
    }

    public float[] getQuestParams(){
        return new float[6] {textureMin, textureMax, warmthMin, warmthMax, flavorMin, flavorMax};   
    }
}