using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChange : MonoBehaviour{
    public Animator Transition;
    public float TransitionTime = 1f;

    /**
        This function will basically swap scenes
        add the scene you want to change to in the parameter
    **/
    public void SceneChangeTo(string SceneToChangeTo){
        StartCoroutine(LoadLevel(SceneToChangeTo));
    }

    IEnumerator LoadLevel(string SceneToChangeTo){
        //Play Anim
        Transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(TransitionTime);

        //Load Scene
        SceneManager.LoadScene(SceneToChangeTo, LoadSceneMode.Single);
    }

}
