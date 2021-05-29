using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkerScript : MonoBehaviour
{
    public AudioClip walkSound;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.AddComponent<AudioSource>(); //adds an AudioSource to the game object this script is attached to
        audio.playOnAwake = false;
        audio.clip = walkSound;
        audio.loop = true;
        audio.Stop();
    }

    // Update is called once per frame
    public void Walk()
    {
        if(!audio.isPlaying)
            audio.Play();
    }

    public void Stop()
    {
        audio.Stop();
    }
}
