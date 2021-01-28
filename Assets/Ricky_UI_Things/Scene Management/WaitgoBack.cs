using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitgoBack : MonoBehaviour{
    // Update is called once per frame
    void Update(){
        if(Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene("SceneSelection", LoadSceneMode.Single);
    }

}
