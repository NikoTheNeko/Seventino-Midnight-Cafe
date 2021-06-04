using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static EnemyBH.DamageEnum;

public class KnifeHandler : MonoBehaviour
{
    //public Transform attackPoint;
    //public float attackRange = 0.25f;
    //public LayerMask enemyLayer;
    public int knifeDamage = 8;
    static public int knfDamage;

    public TrailRenderer trail;

    private void Awake()
    {
        knfDamage = knifeDamage;
        trail.emitting = false;
    }

    public AudioClip knifeSound;
    private AudioSource audio;
    public AudioMixerGroup mixer;
    void Start()
    {
        audio = gameObject.AddComponent<AudioSource>(); //adds an AudioSource to the game object this script is attached to
        audio.outputAudioMixerGroup = mixer;
        audio.playOnAwake = false;
        audio.clip = knifeSound;
        audio.loop = false;
        audio.Stop();
    }
    public void Swing(Vector3 attackPoint, float attackRange, LayerMask enemyLayer)
    {
        audio.Play();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, 1.5f, enemyLayer);
        StartCoroutine(KnifeTrail());
        foreach (Collider2D enemy in hitEnemies)
        {
            //damage = playerScript.RandomDamage(minKnifeDamage, maxKnifeDamage);
            enemy.GetComponent<EnemyBH>().TakeDamage(knfDamage, Slice);
            CameraShake.instance.ShakeCamera(.25f, .03f);
        }
    }

    IEnumerator KnifeTrail()
    {
        Debug.Log("went into ienum");
        trail.emitting = true;
        yield return new WaitForSeconds(0.15f);
        trail.Clear();
        trail.emitting = false;

    }
}
