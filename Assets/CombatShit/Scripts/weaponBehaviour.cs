using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponBehaviour : MonoBehaviour
{
    public int index;
    public GameObject[] weapons;
    public GameObject weaponAnchor;
    public GameObject currentWeapon;

    private int totalWeapons = 1;

    // Start is called before the first frame update
    void Start()
    {
        totalWeapons = weaponAnchor.transform.childCount;
        weapons = new GameObject[totalWeapons];
        for(int i = 0; i < totalWeapons; i++)
        {
            weapons[i] = weaponAnchor.transform.GetChild(i).gameObject;
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);
        currentWeapon = weapons[0];
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            if(index < totalWeapons - 1)
            {
                weapons[index].SetActive(false);
                index++;
                weapons[index].SetActive(true);
            }
            else
            {
                weapons[index].SetActive(false);
                index = 0;
                weapons[index].SetActive(true);
            }
            currentWeapon = weapons[index];
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (index > 0)
            {
                weapons[index].SetActive(false);
                index--;
                weapons[index].SetActive(true);
            }
            else
            {
                weapons[index].SetActive(false);
                index = totalWeapons - 1;
                weapons[index].SetActive(true);
            }
            currentWeapon = weapons[index];
        }
    }
}
