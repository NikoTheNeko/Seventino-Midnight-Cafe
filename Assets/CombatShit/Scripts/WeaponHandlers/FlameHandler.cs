using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static EnemyBH.DamageEnum;

public class FlameHandler : MonoBehaviour
{
    //public Transform attackPoint;
    //public float attackRange = 0.25f;
    //public LayerMask enemyLayer;
    public int fireDamage = 4;
    //public int maxFireDamage = 4;
    public bool canTakeDamage = true;

    public BoxCollider2D flames;

    //public AudioClip flameSound;
    public AudioSource audio;
    //public AudioMixerGroup mixer;

    public int interpolationFramesCount = 30;
    private int elapsedFrames = 0;

    void Start()
    {
        /**
        audio = gameObject.AddComponent<AudioSource>(); //adds an AudioSource to the game object this script is attached to
        audio.outputAudioMixerGroup = mixer;
        audio.playOnAwake = false;
        audio.clip = flameSound;
        audio.loop = true;
        audio.Stop();
        **/
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) && !audio.isPlaying)
        {
            audio.Play();
        }
        else if(Input.GetMouseButton(0) && audio.isPlaying)
        {

        }
        else
        {
            audio.Stop();
        }

        if(Input.GetMouseButtonDown(0))
            canTakeDamage = true;
    }

    private void OnTriggerStay2D(Collider2D enemy)
    {
        if (enemy.gameObject.tag == "enemy")
        {
            if (canTakeDamage)
            {
                StartCoroutine(WaitForSeconds());
                //enemy.gameObject.GetComponent<EnemyBH>().TakeDamage(Random.Range(minFireDamage, maxFireDamage + 1), Fire);
                enemy.gameObject.GetComponent<EnemyBH>().TakeDamage(fireDamage, Fire);
                CameraShake.instance.ShakeCamera(.2f, .015f);
                if (enemy.transform.childCount < 1)
                {
                    CameraShake.instance.ShakeCamera(.2f, .015f); 
                }
            }
        }
    }
    IEnumerator WaitForSeconds()
    {
        canTakeDamage = false;
        yield return new WaitForSecondsRealtime(0.1f);
        canTakeDamage = true;
    }
    public void ActivateFlame()
    {
        flames.size = new Vector2(19, 6);
    }

    public void DeactivateFlame()
    {
        flames.size = new Vector2(0, 0);
    }
}
