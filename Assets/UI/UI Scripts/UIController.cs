using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameObject player;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        healthControl();
    }

    public void healthControl(){
        int counter = 0;
        int health = player.GetComponent<PlayerCombatTesting>().health;
        Debug.Log("healt: " + health);
        // Debug.Log(health/20);
        while(counter < (int)(health/20)){
            Debug.Log(counter);
            hearts[counter].sprite = fullHeart;
            counter++;
        }
        if(health%20 != 0){
            hearts[counter].sprite = halfHeart;
            counter++;
        }
        while(counter < hearts.Length){
            hearts[counter].sprite = emptyHeart;
            counter++;
        }
    }
}
