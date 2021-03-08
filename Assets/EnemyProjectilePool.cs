using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectilePool : MonoBehaviour
{
    public static EnemyProjectilePool beanPoolInstanse;

    [SerializeField]
    private GameObject pooledBeans;
    private bool needBeansInPool = true;

    private List<GameObject> beans;


    private void Awake()
    {
        beanPoolInstanse = this;
        Debug.Log("POOL IS ALIVE");
    }


    // Start is called before the first frame update
    void Start()
    {
        beans = new List<GameObject>();
    }


    public GameObject GetBean()
    {
        if (beans.Count > 0)
        {
            for (int i = 0; i < beans.Count; i++)
            {
                if (!beans[i].activeInHierarchy)
                {
                    return beans[i];
                }
            }
        }

        if (needBeansInPool)
        {
            GameObject bean = Instantiate(pooledBeans);
            bean.SetActive(false);
            beans.Add(bean);
            return bean;
        }
        return null;
    }
}
