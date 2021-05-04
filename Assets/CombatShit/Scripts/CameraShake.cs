using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation;
    public float rotationMultiplier = 3;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float x = Random.Range(-1f, 1f) * shakePower;
            float y = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(x, y, 0f);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1f, 1f));
    }

    public void ShakeCamera(float shakeLength, float shkPower)
    {
        shakeTimeRemaining = shakeLength;
        shakePower = shkPower;

        shakeFadeTime = shkPower/shakeLength;
        shakeRotation = shkPower * rotationMultiplier;
    }
}
