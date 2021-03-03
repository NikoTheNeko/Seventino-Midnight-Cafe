using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHandler : MonoBehaviour
{
    //public Transform attackPoint;
    //public float attackRange = 0.25f;
    //public LayerMask enemyLayer;
    //public int damage;

    public static void Swing(Vector3 attackPoint, float attackRange, LayerMask enemyLayer)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, 1.5f, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyBehaviour>().TakeDamage(10, DamageEnum.Slice);
            CameraShake.instance.ShakeCamera(.25f, .03f);
        }
    }
}
