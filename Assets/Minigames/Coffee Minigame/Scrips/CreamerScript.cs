using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreamerScript : MonoBehaviour{
  #region Public Variables

    [Header("Minigame Objects and Variables")]
    [Tooltip("The amount you will add to Flavor")]
    //Values forr when you add the flavors
    public int[] FlavorValues = {0, 2, 5};

    [Tooltip("The amount you will add to Texture")]
    public int[] TextureValues = {10, 8, 5};
    [Tooltip("Amount of Glug")]
    public int GlugLeft = 30;


    //[Tooltip("Creamer SFX")]
    public AudioSource CreamerSFX;
   // public Animator Animation;

    [Tooltip("The locations of each Creamer bottle")]
    public Transform[] CreamerLocations = new Transform[3];

    [Tooltip("The cup that moves")]
    public Transform Cup;

    [Header("UI and Stat Manager")]
    [Tooltip("This is the manager for the stats so we can update them")]
    public GameObject StatManager;

    [Tooltip("This is the cooking manager so it manages the minigames and order etc")]
    public GameObject CookingManager;

    [Tooltip("This is the instructions so players know what to fuckin do")]
    public Text Instructions;

    [Tooltip("This is the minigame Canvas")]
    public GameObject MinigameCanvas;

    public Slider MilkSlider;
    public Transform Milk;
    public ParticleSystem MilkPS;
    public Slider CookiesSlider;
    public Transform Cookies;
    public ParticleSystem CookiesPS;

    public Slider GlugSlider;

    public float TiltAngle = 45f;

    #endregion

    #region Private Variables
    //States for the Creamer
    //0 - Vanilla
    //1 - Caramel
    //2 - Chocolate
    private int CreamerState = 0;
    
    //This is so that you can't play the game if it's not active
    private bool MinigameActive = false;

    private float AddedAmount = 0;

    #endregion


    void Start() {
    }

    // Update is called once per frame
    void Update(){
        TiltCreamer(Milk, MilkSlider);
        TiltCreamer(Cookies, CookiesSlider);
        if(MinigameActive){
            MinigameCanvas.SetActive(true);
            RunMinigame();
        } else {
            CookiesPS.Stop();
            MilkPS.Stop();
            MinigameCanvas.SetActive(false);
            CreamerState = 0;
            ResetSlider(MilkSlider);
            ResetSlider(CookiesSlider);
            MilkPS.Stop();
            CookiesPS.Stop();
        }
        CreamerSFX.volume = CookiesSlider.value + MilkSlider.value;
    }

    #region Minigame Function
    /**
        Since this minigame is so small I shoved everything into one function.
        It just adds the amount from AddAmount to the thing. That's it.
    **/
    private void RunMinigame(){
        ShowPluses();
        MoveCup();

        if(AddedAmount <= GlugLeft){
            Instructions.text = "Press left and right to select a creamer and press add to add!";
            StatManager.GetComponent<FoodStats>().UpdateFlavorPreview(StatManager.GetComponent<FoodStats>().FlavorVal + FlavorValues[CreamerState]);
            StatManager.GetComponent<FoodStats>().UpdateWarmthPreview(0);
            StatManager.GetComponent<FoodStats>().UpdateTexturePreview(StatManager.GetComponent<FoodStats>().TextureVal + TextureValues[CreamerState]);
            
            if(CreamerState == 0){
                AddCreamer(Milk, MilkSlider, MilkPS);
                CookiesSlider.interactable = false;
                ResetSlider(CookiesSlider);
                CookiesPS.Stop();
            } else if (CreamerState == 1){
                AddCreamer(Cookies, CookiesSlider, CookiesPS);
                MilkSlider.interactable = false;
                ResetSlider(MilkSlider);
                MilkPS.Stop();
            }

            GlugSlider.value = 1 - (AddedAmount / GlugLeft);

        } else {
            Instructions.text = "You can't add anymore creamer!";
            CookiesPS.Stop();
            MilkPS.Stop();
        }
        
        CookingManager.GetComponent<CookingController>().MinigameFinished(5, true);
    }

    //Called by the button to pump Creamer
    public void AddCreamer(Transform Pitcher, Slider PouringSlider, ParticleSystem PS){
        PouringSlider.interactable = true;
        if(PouringSlider.value > 0){
            if(!PS.isPlaying)
                PS.Play();
            StatManager.GetComponent<FoodStats>().AddFlavor(FlavorValues[CreamerState] * PouringSlider.value * Time.deltaTime);
            StatManager.GetComponent<FoodStats>().AddTexture(TextureValues[CreamerState] * PouringSlider.value * Time.deltaTime);
            AddedAmount += 5 * PouringSlider.value * Time.deltaTime;
        } else if(PouringSlider.value == 0) {
            if(PS.isPlaying)
                PS.Stop();
        }
    }

    //Called by the buttons to move the cup and change Creamers
    //Moves the state left (-1) or right (1)
    public void AdjustSelection(int direction){
        if(CreamerState + direction >= 0 && CreamerState + direction <= 1){
            CreamerState += direction;
        }
    }

    //This moves the coffee cup to the correct location
    private void MoveCup(){
        Vector3 NewLocation = Vector3.Lerp(Cup.position, CreamerLocations[CreamerState].position, 0.2f);
        Cup.position = NewLocation;
    }


    //This hides and shows pluses based on the 
    private void ShowPluses(){
        StatManager.GetComponent<FoodStats>().ShowPlus(0);
        StatManager.GetComponent<FoodStats>().HidePlus(1);
        StatManager.GetComponent<FoodStats>().ShowPlus(2);    

    }

    //Resets the sliders
    private void ResetSlider(Slider MinigameSlider){
        MinigameSlider.value -= 5f * Time.deltaTime;
    }


    #endregion

    #region Animation Controls

    public void PressedFinished()
    {
    //    Animation.SetBool("Pressed", false);
    }

    /**
        This tilts the pitcher the higher you bring it up
    **/
    private void TiltCreamer(Transform Pitcher, Slider PouringSlider){
        float NewAngle = TiltAngle * PouringSlider.value;
        Pitcher.rotation = Quaternion.AngleAxis(NewAngle, Vector3.forward);
    }

    #endregion

    #region Cooking Controller Actions
    //ActivateMinigame gets called to activate the minigame so it can actually run
    public void ActivateMinigame(){
        MinigameActive = true;
    }

    //DeactivateMinigame turns the minigame off so you can't play it
    public void DeactivateMinigame(){
        MinigameActive = false;
    }
    #endregion

}
