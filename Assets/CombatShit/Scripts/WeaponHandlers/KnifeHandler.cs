using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBH.DamageEnum;

public class KnifeHandler : MonoBehaviour
{
    //public Transform attackPoint;
    //public float attackRange = 0.25f;
    //public LayerMask enemyLayer;
    public static int knifeDamage = 8;

    public static void Swing(Vector3 attackPoint, float attackRange, LayerMask enemyLayer)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, 1.5f, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyBH>().TakeDamage(knifeDamage, Slice);
            CameraShake.instance.ShakeCamera(.25f, .03f);
        }
    }
}
