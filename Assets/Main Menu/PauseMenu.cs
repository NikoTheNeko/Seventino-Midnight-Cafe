using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public static bool QuestsUp = false;
    public static bool MapUp = false;
    public static bool isOpen = false;
    public Slider volumeSlider;
    public float volume = 1f;
    public AudioSource audio;
    public AudioMixer allAudio;
    private InventoryTracker tracker;
    public GameObject pauseMenuUI;

    void Start(){
        //set volume to and volume slider to volume in inventory tracker, should be valued carried from previous scenes
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        volume = tracker.volume;
        volumeSlider.value = volume;
        audio.volume = volume;
        allAudio.SetFloat("Volume", volume);
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void GoHome()
    {
        UnPauseTime();
        IsPaused = false;
        SceneManager.LoadScene("Cafe Walk Around");
    }

    public void TitleScreen()
    {
        UnPauseTime();
        IsPaused = false;
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitGame()
    {
        UnPauseTime();
        Application.Quit();
    }

    public void UnPauseTime(){
        Time.timeScale = 1f;
    }

    public void ChangeVolume(){
        volume = volumeSlider.value;
        tracker.volume = volume;
        if(audio != null){
            audio.volume = volume;
            allAudio.SetFloat("Volume", Mathf.Log10(volume) * 20);
        }
        // audio.Play();
    }

    public void SaveGame(){
        tracker.save();
    }

    public void LoadGame(){
        tracker.load();
    }
}
