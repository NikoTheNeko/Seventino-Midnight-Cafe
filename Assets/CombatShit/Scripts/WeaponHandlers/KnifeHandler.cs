using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBH.DamageEnum;

public class KnifeHandler : MonoBehaviour
{
    //public Transform attackPoint;
    //public float attackRange = 0.25f;
    //public LayerMask enemyLayer;
    public int knifeDamage = 8;
    static public int knfDamage;

    public AudioClip knifeSound;
    private AudioSource audio;
    void Start()
    {
        knfDamage = knifeDamage; 
        audio = gameObject.AddComponent<AudioSource>(); //adds an AudioSource to the game object this script is attached to
        audio.playOnAwake = false;
        audio.clip = knifeSound;
        audio.loop = false;
        audio.Stop();
    }
    public void Swing(Vector3 attackPoint, float attackRange, LayerMask enemyLayer)
    {
        audio.Play();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, 1.5f, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyBH>().TakeDamage(knfDamage, Slice);
            CameraShake.instance.ShakeCamera(.25f, .03f);
        }
    }
}
