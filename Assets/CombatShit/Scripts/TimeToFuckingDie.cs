using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToFuckingDie : MonoBehaviour
{
    public float timerLength = 1f;

    public Collider2D enemy;

    public BoxCollider2D trigger;

    // Update is called once per frame
    void Update(){

        if(timerLength > 0){
            timerLength -= Time.deltaTime;
        } else {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        other.gameObject.SendMessage("TakeDamage");
    }


}
