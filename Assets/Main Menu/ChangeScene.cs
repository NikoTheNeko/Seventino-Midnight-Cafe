using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour{

    public void ChangeSceneTo(string SceneName){
        string SceneToLoad = SceneName;
        SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
    }

}
