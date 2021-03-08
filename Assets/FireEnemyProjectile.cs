using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyProjectile : MonoBehaviour
{
    [SerializeField]
    public int beansAmount = 3;

    [SerializeField]
    public float startAngle = 0f, endAngle = 45f;


    private Vector2 beanMoveDirection;

    private void Start()
    {
        InvokeRepeating("Fire", 0f, 2f);
    }
    //Transform target, int beansAmount, float startAngle, float endAngle
    public void Fire()
    {
        float angleStep = (endAngle - startAngle) / beansAmount;
        float angle = startAngle;

        for (int i = 0; i < beansAmount; i++)
        {
            float beanDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float beanDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 beanMoveVector = new Vector3(beanDirectionX, beanDirectionY, 0f);
            Vector2 beanDirection = (beanMoveVector - transform.position).normalized;

            GameObject bean = EnemyProjectilePool.beanPoolInstanse.GetBean();
            bean.transform.position = transform.position;
            bean.transform.rotation = transform.rotation;
            bean.SetActive(true);
            bean.GetComponent<EnemyProjectile>().SetMoveDirection(beanDirection);

            angle =+ angleStep;
        }
    }
}
