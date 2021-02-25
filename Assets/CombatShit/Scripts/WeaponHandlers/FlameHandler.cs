﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameHandler : MonoBehaviour
{
    //public Transform attackPoint;
    //public float attackRange = 0.25f;
    //public LayerMask enemyLayer;
    //public int damage;
    public bool canTakeDamage = true;

    public BoxCollider2D flames;

    public int interpolationFramesCount = 30;
    private int elapsedFrames = 0;

    private void OnTriggerStay2D(Collider2D enemy)
    {
        Debug.Log("fujck");
        if (enemy.gameObject.tag == "enemy")
        {
            if (canTakeDamage)
            {
                StartCoroutine(WaitForSeconds());
                enemy.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(1);
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