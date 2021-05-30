using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBH.DamageEnum;

public class hurtParticleScript : MonoBehaviour
{
    public bool hurtyZone = true;


    public AudioClip hurtSound;
    private AudioSource audio;
    public ParticleSystem blood;

    public GameObject player;

    public int interpolationFramesCount = 30;
    private int elapsedFrames = 0;

    void Start()
    {
        audio = gameObject.AddComponent<AudioSource>(); //adds an AudioSource to the game object this script is attached to
        audio.playOnAwake = false;
        audio.clip = hurtSound;
        audio.loop = true;
        audio.Stop();
        var bl = blood.emission;
        bl.enabled = false;
    }

    private void Update()
    {
        if (player.GetComponent<PlayerCombatTesting>().health <= 3 && !audio.isPlaying)
        {
            audio.Play();
            var bl = blood.emission;
            bl.enabled = true;
        }
        else if (player.GetComponent<PlayerCombatTesting>().health <= 3 && audio.isPlaying)
        {

        }
        else
        {
            audio.Stop();
            var bl = blood.emission;
            bl.enabled = false;
        }
    }
}
