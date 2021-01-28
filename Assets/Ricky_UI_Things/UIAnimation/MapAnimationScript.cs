using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimationScript : MonoBehaviour
{
    public static bool mapIsOpen = false;

    public GameObject Map;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            OpenMap();
        }
    }

    public void OpenMap()
    {
        Animator mapAnimator = Map.GetComponent<Animator>();
        if (mapAnimator != null)
        {
            mapIsOpen = mapAnimator.GetBool("open");

            mapAnimator.SetBool("open", !mapIsOpen);
        }
    }
}
