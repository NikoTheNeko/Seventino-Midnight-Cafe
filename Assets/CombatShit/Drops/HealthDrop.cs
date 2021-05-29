using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("triggered");
        if(other.gameObject.tag == "Player"){
            PlayerCombatTesting player = other.gameObject.GetComponent<PlayerCombatTesting>();
            if(player.health < 10){
                player.health += 2;
                if(player.health > 10){
                    player.health = 10;
                }
                Destroy(this.gameObject);
            } 
        }
    }
}
