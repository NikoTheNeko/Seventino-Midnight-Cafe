using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bean : MonoBehaviour
{
    public Vector2 velocity;
    public float speed;
    public float rotation;
    public float beanLife;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = beanLife;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * speed * Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0) gameObject.SetActive(false);
    }

    public void ResetTimer()
    {
        timer = beanLife;
    }

    public void SetRotation(float r)
    {
        transform.rotation = Quaternion.Euler(0, 0, r);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerCombatTesting>().PlayerHit(1);
            CameraShake.instance.ShakeCamera(.25f, .05f);
            //Debug.Log(health);
            gameObject.SetActive(false);
        }
    }
}
