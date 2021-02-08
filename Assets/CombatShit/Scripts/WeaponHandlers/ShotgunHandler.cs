using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ShotgunHandler : MonoBehaviour
{
    public LineRenderer tracer;
    [SerializeField] private Material weaponTracerMaterial;
    [SerializeField] private Sprite shootFlashSprite;
    public void RayShoot(Vector3 EndPoint, Vector3 ShootDir)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(EndPoint, ShootDir);
        CreateWeaponTracer(EndPoint, EndPoint, ShootDir);
        CreateShootFlash(EndPoint);
        if (raycastHit2D.collider != null)
        {
            EnemyBehaviour target = raycastHit2D.collider.GetComponent<EnemyBehaviour>();
            if (target != null)
            {
                target.TakeDamage(5);
            }
        }
    }

    public void CreateWeaponTracer(Vector3 fromPosition, Vector3 targetPosition, Vector3 targetNormal)
    {
        targetNormal = targetNormal * Camera.main.orthographicSize * 2.1f;
        tracer.enabled = true;
        tracer.SetPosition(0, fromPosition);
        tracer.SetPosition(1, targetPosition + targetNormal);
        FunctionTimer.Create(turnoffTracer, .05f);
    }

    public void turnoffTracer()
    {
        tracer.enabled = false;
    }

    private void CreateShootFlash(Vector3 spawnPosition)
    {
        World_Sprite worldSprite = World_Sprite.Create(spawnPosition, Vector3.zero, shootFlashSprite, Color.white, 10000);
        FunctionTimer.Create(worldSprite.DestroySelf, .05f);
    }

    public static void ShakeCamera(float intensity, float timer)
    {
        Vector3 lastCameraMovement = Vector3.zero;
        FunctionUpdater.Create(delegate () {
            timer -= Time.unscaledDeltaTime;
            Vector3 randomMovement = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * intensity;
            Camera.main.transform.position = Camera.main.transform.position - lastCameraMovement + randomMovement;
            lastCameraMovement = randomMovement;
            return timer <= 0f;
        }, "CAMERA_SHAKE");
    }
}
