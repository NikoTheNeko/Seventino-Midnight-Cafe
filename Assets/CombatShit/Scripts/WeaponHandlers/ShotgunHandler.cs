using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ShotgunHandler : MonoBehaviour
{
    private Material weaponTracerMaterial;
    public static void RayShoot(Vector3 EndPoint, Vector3 ShootDir)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(EndPoint, ShootDir);
        //WeaponTracer.Create()
        if (raycastHit2D.collider != null)
        {
            EnemyBehaviour target = raycastHit2D.collider.GetComponent<EnemyBehaviour>();
            if (target != null)
            {
                target.TakeDamage(5);
            }
        }
    }

    public void CreateWeaponTracer(Vector3 fromPosition, Vector3 targetPosition)
    {
        Vector3 dir = (targetPosition - fromPosition).normalized;
        //float eulerZ = GetAngleFromVectorFloat(dir) - 90;
        float distance = Vector3.Distance(fromPosition, targetPosition);
        Vector3 spawnPos = fromPosition + dir * distance * .5f;
        Material tmpMaterial = new Material(weaponTracerMaterial);
        tmpMaterial.SetTextureScale("_MainTex", new Vector2(1f, distance / 256f));
        //World_Mesh worldMesh = World_Mesh.Create(spawnPos, eulerZ, 6f, distance, tmpMaterial, null, 10000);
        /*
        float timer = .1f;
        Time -= Time.deltaTime;
        if(timer <= 0)
        {
            worldMesh.DestroySelf();
            return true;
        }*/
    }
}
