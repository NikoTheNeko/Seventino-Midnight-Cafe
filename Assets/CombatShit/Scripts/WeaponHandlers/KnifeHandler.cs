using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBH.DamageEnum;

public class KnifeHandler : MonoBehaviour
{
    //public Transform attackPoint;
    //public float attackRange = 0.25f;
    //public LayerMask enemyLayer;
    public int minKnifeDamage = 6;
    public int maxKnifeDamage = 8;
    private int damage;
    PlayerCombatTesting playerScript;
    

    public AudioClip knifeSound;
    private AudioSource audio;
    void Start()
    {
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
            damage = playerScript.RandomDamage(minKnifeDamage, maxKnifeDamage);
            enemy.GetComponent<EnemyBH>().TakeDamage(damage, Slice);
            CameraShake.instance.ShakeCamera(.25f, .03f);
        }
    }
}
