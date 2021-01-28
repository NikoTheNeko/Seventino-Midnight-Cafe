using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAnimationScript : MonoBehaviour
{
    public static bool questsIsOpen = false;

    public GameObject Quests;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            OpenQuests();
        }
    }

    public void OpenQuests()
    {
        Debug.Log("uh oh stinky");
        Animator animator = Quests.GetComponent<Animator>();
        if (animator != null)
        {
            Debug.Log("fuckshitpoopassssssssssssss");
            questsIsOpen = animator.GetBool("open");

            animator.SetBool("open", !questsIsOpen);
        }
    }
}
