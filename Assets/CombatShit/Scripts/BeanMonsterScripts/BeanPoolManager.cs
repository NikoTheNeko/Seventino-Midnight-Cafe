using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanPoolManager : MonoBehaviour
{
    public static List<GameObject> beans;

    // Start is called before the first frame update
    void Start()
    {
        beans = new List<GameObject>();
    }

    public static GameObject GetBeanFromPool()
    {
        for (int i = 0; i < beans.Count; i++)
        {
            if (!beans[i].active)
            {
                //beans[i].GetComponent<Bean>().ResetTimer();
                beans[i].SetActive(true);
                return beans[i];
            }
        }
        return null;
    }
    /*
    public static GameObject GetBeanFromPoolWithType(string type)
    {
        for (int i = 0; i < beans.Count; i++)
        {
            if (!beans[i].active && beans[i].GetComponent<Bean>().GetType)
                return beans[i];
        }
        return null;
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
