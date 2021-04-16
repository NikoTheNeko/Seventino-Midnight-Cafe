using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using static EnemyBH.DamageEnum;
public class ShotgunHandler : MonoBehaviour
{
    public List<LineRenderer> tracers = new List<LineRenderer>(5);
    [SerializeField] private Material weaponTracerMaterial;
    [SerializeField] public SpriteRenderer muzzleFlash;
    public Action<LineRenderer> turnOff;
    public bool canFire = true;
    public Animator animator;

    public AudioClip shotSound;
    private AudioSource audio;
    void Start()
    {
        audio = gameObject.AddComponent<AudioSource>(); //adds an AudioSource to the game object this script is attached to
        audio.playOnAwake = false;
        audio.clip = shotSound;
        audio.loop = false;
        audio.Stop();
    }
    public void RayShoot(Vector3 EndPoint, Vector3 ShootDir)
    {
        if (canFire)
        {
            animator.SetTrigger("Shoot");
            audio.Play();
            canFire = false;
            foreach (LineRenderer tracer in tracers)
            {
                float angle = UnityEngine.Random.Range(-5f, 5f);
                Vector3 newShootDir = (Quaternion.Euler(0f, 0f, angle) * ShootDir);
                RaycastHit2D raycastHit2D = Physics2D.Raycast(EndPoint, newShootDir);
                CreateWeaponTracer(EndPoint, EndPoint, newShootDir, tracer);
                CreateShootFlash();
                if (raycastHit2D.collider != null)
                {
                    EnemyBH target = raycastHit2D.collider.GetComponent<EnemyBH>();
                    if (target != null)
                    {
                        target.TakeDamage(5, Flavor);
                        CameraShake.instance.ShakeCamera(.5f, .02f);
                    }
                }
            }
            StartCoroutine(ResetFire(0.5f));
        }
    }

    IEnumerator ResetFire(float time)
    {
        yield return new WaitForSeconds(time);
        canFire = true;
    }

    public void CreateWeaponTracer(Vector3 fromPosition, Vector3 targetPosition, Vector3 targetNormal, LineRenderer tracer)
    {
        targetNormal = targetNormal * Camera.main.orthographicSize * 2.1f;
        tracer.enabled = true;
        tracer.SetPosition(0, fromPosition);
        tracer.SetPosition(1, targetPosition + targetNormal);
        StartCoroutine(KillTrace(0.08f, tracer));

    }
    IEnumerator KillTrace(float time, LineRenderer tracer)
    {
        yield return new WaitForSeconds(time);
        tracer.enabled = false;
    }

    private void CreateShootFlash()
    {
        muzzleFlash.enabled = true;
        StartCoroutine(killFlash(0.03f, muzzleFlash));
    }

    IEnumerator killFlash(float time, SpriteRenderer muzzleFlash)
    {
        yield return new WaitForSeconds(time);
        muzzleFlash.enabled = false;
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
