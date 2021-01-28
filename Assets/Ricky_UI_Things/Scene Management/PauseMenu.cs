using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public static bool QuestsUp = false;
    public static bool MapUp = false;
    public static bool isOpen = false;

    public GameObject pauseMenuUI;
    //public GameObject questButton;
    //public GameObject questList;
    //public GameObject mapButton;
    //public GameObject map;

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
        //    if(Input.GetKeyDown(KeyCode.Q))
        //    {
        //        if (QuestsUp)
        //        {
        //            //PutDownQuests();
        //            //OpenQuest();
        //        } else
        //        {
        //            //PullUpQuests();
        //            //OpenQuest();
        //        }
        //    }
        //    if(Input.GetKeyDown(KeyCode.M))
        //    {
        //        if(MapUp)
        //        {
        //            PutDownMap();
        //        } else
        //        {
        //            PullUpMap();
        //        }
        //    }
        //}

        //public void PutDownQuests()
        //{
        //    Debug.Log("FUCK");
        //    questList.SetActive(false);
        //    questButton.SetActive(true);
        //    QuestsUp = false;
        //}

        //public void PullUpQuests()
        //{
        //    Debug.Log("SHIT");
        //    questList.SetActive(true);
        //    questButton.SetActive(false);
        //    QuestsUp = true;
        //}

        //public void PutDownMap()
        //{
        //    map.SetActive(false);
        //    mapButton.SetActive(true);
        //    MapUp = false;
        //}

        //public void PullUpMap()
        //{
        //    map.SetActive(true);
        //    mapButton.SetActive(false);
        //    MapUp = true;
        //}
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
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene("SceneSelection");
    }

    public void TitleScreen()
    {
        Debug.Log("poopfuck titlescreen");
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitGame()
    {
        Debug.Log("quitting poopfuck");
        Application.Quit();
    }
}
