using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAnimationScript : MonoBehaviour
{
    public static bool IsOpen = false;

    public GameObject Inventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }
    }

    public void OpenInventory()
    {
        Animator animator = Inventory.GetComponent<Animator>();
        if (animator != null)
        {
            IsOpen = animator.GetBool("open");

            animator.SetBool("open", !IsOpen);
        }
    }
}
