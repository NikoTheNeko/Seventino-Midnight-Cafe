using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBH.DamageEnum;

public class FlameHandler : MonoBehaviour
{
    //public Transform attackPoint;
    //public float attackRange = 0.25f;
    //public LayerMask enemyLayer;
    public int fireDamage = 3;
    public bool canTakeDamage = true;

    public BoxCollider2D flames;

    public int interpolationFramesCount = 30;
    private int elapsedFrames = 0;

    private void OnTriggerStay2D(Collider2D enemy)
    {
        if (enemy.gameObject.tag == "enemy")
        {
            if (canTakeDamage)
            {
                StartCoroutine(WaitForSeconds());
                enemy.gameObject.GetComponent<EnemyBH>().TakeDamage(fireDamage, Fire);
                CameraShake.instance.ShakeCamera(.2f, .015f);
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
