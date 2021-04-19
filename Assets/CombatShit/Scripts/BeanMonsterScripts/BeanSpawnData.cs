using UnityEngine;
[CreateAssetMenu(fileName = "Date", menuName ="ScriptableObjects/BulletSpawnData", order = 1)]

public class BeanSpawnData : ScriptableObject
{
    public GameObject beanResource;
    public float minRotation;
    public float maxRotation;
    public int numberOfBeans;
    public bool isRandom;
    public bool isParent;
    public float cooldown;
    public float beanSpeed;
    public Vector2 beanVelocity;
}
