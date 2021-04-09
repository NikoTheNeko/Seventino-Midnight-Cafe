using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanSpawner : MonoBehaviour
{
    public GameObject beanResource;
    public float minRotation;
    public float maxRotation;
    public int numberOfBullets;
    public bool isRandom;

    public float cooldown;
    float timer;
    public float bulletSpeed;
    public Vector2 bulletVelocity;


    float[] rotations;
    void Start()
    {
        timer = cooldown;
        rotations = new float[numberOfBullets];
        if (!isRandom)
        {
            /* 
             * This doesn't need to be in update because the rotations will be the same no matter what
             * Unless if we change min Rotation and max Rotation Variables leave this in Start.
             */
            DistributedRotations();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            SpawnBullets();
            timer = cooldown;
        }
        timer -= Time.deltaTime;
    }

    // Select a random rotation from min to max for each bullet
    public float[] RandomRotations()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            rotations[i] = Random.Range(minRotation, maxRotation);
        }
        return rotations;

    }

    // This will set random rotations evenly distributed between the min and max Rotation.
    public float[] DistributedRotations()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            var fraction = (float)i / ((float)numberOfBullets - 1);
            var difference = maxRotation - minRotation;
            var fractionOfDifference = fraction * difference;
            rotations[i] = fractionOfDifference + minRotation; // We add minRotation to undo Difference
        }
        foreach (var r in rotations) print(r);
        return rotations;
    }
    public GameObject[] SpawnBullets()
    {
        if (isRandom)
        {
            // This is in Update because we want a random rotation for each bullet each time
            RandomRotations();
        }

        // Spawn Bullets
        GameObject[] spawnedBullets = new GameObject[numberOfBullets];
        for (int i = 0; i < numberOfBullets; i++)
        {
            spawnedBullets[i] = Instantiate(beanResource, transform);
            Debug.Log(transform.position);

            var b = spawnedBullets[i].GetComponent<Bean>();
            b.rotation = rotations[i];
            b.speed = bulletSpeed;
            b.velocity = bulletVelocity;
        }
        return spawnedBullets;
    }
    /*
    public BeanSpawnData[] spawnData;
    int index = 0;
    public bool isSequenceRandom; 

    BeanSpawnData GetSpawnData()
    {
        return spawnData[index];
    }

    float timer;

    float[] rotations;
    void Start()
    {
        timer = GetSpawnData().cooldown;
        rotations = new float[GetSpawnData().numberOfBeans];
        if (!GetSpawnData().isRandom)
        {
            /* 
             * This doesn't need to be in update because the rotations will be the same no matter what
             * Unless if we change min Rotation and max Rotation Variables leave this in Start.
             *
            DistributedRotations();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            SpawnBeans();
            timer = GetSpawnData().cooldown;
            if (isSequenceRandom)
            {
                index = Random.Range(0, spawnData.Length);
            }
            else
            {
                index++;
                if (index >= spawnData.Length) index = 0;
            }
        }
        timer -= Time.deltaTime;
    }

    // Select a random rotation from min to max for each bean
    public float[] RandomRotations()
    {
        for (int i = 0; i < GetSpawnData().numberOfBeans; i++)
        {
            rotations[i] = Random.Range(GetSpawnData().minRotation, GetSpawnData().maxRotation);
        }
        return rotations;

    }

    // This will set random rotations evenly distributed between the min and max Rotation.
    public float[] DistributedRotations()
    {
        for (int i = 0; i < GetSpawnData().numberOfBeans; i++)
        {
            var fraction = (float)i / ((float)GetSpawnData().numberOfBeans - 1);
            var difference = GetSpawnData().maxRotation - GetSpawnData().minRotation;
            var fractionOfDifference = fraction * difference;
            rotations[i] = fractionOfDifference + GetSpawnData().minRotation; // We add minRotation to undo Difference
        }
        foreach (var r in rotations) print(r);
        return rotations;
    }
    public GameObject[] SpawnBeans()
    {
        if (GetSpawnData().isRandom)
        {
            // This is in Update because we want a random rotation for each bean each time
            RandomRotations();
        } else
        {
            DistributedRotations();
        }

        // Spawn Beans
        GameObject[] spawnedBeans = new GameObject[GetSpawnData().numberOfBeans];
        for (int i = 0; i < GetSpawnData().numberOfBeans; i++)
        {
            spawnedBeans[i] = BeanPoolManager.GetBeanFromPool();
            if (spawnedBeans[i] == null)
            {
                spawnedBeans[i] = Instantiate(GetSpawnData().beanResource, transform);
                BeanPoolManager.beans.Add(spawnedBeans[i]);
            } else
            {
                spawnedBeans[i].transform.SetParent(transform);
                spawnedBeans[i].transform.localPosition = Vector2.zero;
            }

            var b = spawnedBeans[i].GetComponent<Bean>();
            b.rotation = rotations[i];
            b.speed = GetSpawnData().beanSpeed;
            b.velocity = GetSpawnData().beanVelocity;
            if (GetSpawnData().isParent == true)
            {
                spawnedBeans[i].transform.SetParent(null);
            }
        }
        return spawnedBeans;
    }
    */
}