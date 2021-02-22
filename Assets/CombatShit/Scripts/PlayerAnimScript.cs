using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : MonoBehaviour
{
    public Animator playerAnim;

    private void Awake()
    {
        playerAnim = transform.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            playerAnim.SetInteger("moveIndex", 1);
        }
        else
        {
            playerAnim.SetInteger("moveIndex", 0);
        }
    }
}
