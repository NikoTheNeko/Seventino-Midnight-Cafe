﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public static bool QuestsUp = false;
    public static bool MapUp = false;
    public static bool isOpen = false;
    public Slider volumeSlider;
    public float volume = 1f;
    public AudioSource audio;
    private InventoryTracker tracker;

    public GameObject pauseMenuUI;
    //public GameObject questButton;
    //public GameObject questList;
    //public GameObject mapButton;
    //public GameObject map;

    void Start(){
        tracker = GameObject.FindGameObjectWithTag("InventoryTracker").GetComponent<InventoryTracker>();
        volume = tracker.volume;
        volumeSlider.value = volume;
        audio.volume = volume;
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

        
        // ChangeVolume();
        
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
        Debug.Log("loading poopfuck");
        UnPauseTime();
        IsPaused = false;
        SceneManager.LoadScene("Cafe Walk Around");
    }

    public void TitleScreen()
    {
        Debug.Log("poopfuck titlescreen");
        UnPauseTime();
        IsPaused = false;
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitGame()
    {
        Debug.Log("quitting poopfuck");
        UnPauseTime();
        Application.Quit();
    }

    public void UnPauseTime(){
        Time.timeScale = 1f;
    }

    public void ChangeVolume(){
        volume = volumeSlider.value;
        Debug.Log(volume);
        tracker.volume = volume;
        if(audio != null){
            audio.volume = volume;
        }
        // audio.Play();
    }
}
