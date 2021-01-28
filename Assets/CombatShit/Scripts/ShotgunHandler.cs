using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunHandler : MonoBehaviour
{
    public static void RayShoot(Vector3 EndPoint, Vector3 ShootDir)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(EndPoint, ShootDir);
        WeaponTracer.Create()
        if(raycastHit2D.collider != null)
        {
            EnemyBehaviour target = raycastHit2D.collider.GetComponent<EnemyBehaviour>();
            if(target != null)
            {
                target.TakeDamage(5);
            }
        }
    }
}
