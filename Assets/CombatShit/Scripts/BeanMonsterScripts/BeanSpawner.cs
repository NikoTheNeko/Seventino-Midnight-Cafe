using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanSpawner : MonoBehaviour
{
    public Transform playerTarget;

    public BeanSpawnData[] spawnData;
    int index = 0;
    public bool isSequenceRandom; 

    BeanSpawnData GetSpawnData()
    {
        return spawnData[index];
    }

    float timer;
    int rotationSpeed = 20;
    

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
             */
            DistributedRotations();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("INDEX:");
        //Debug.Log(index);
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
    public void DistributedRotations()
    {
        for (int j = 0; j < rotations.Length; j++)
        {
            rotations[j] = 0;
        }
        //Debug.Log(GetSpawnData().numberOfBeans);
        if(GetSpawnData().numberOfBeans > rotations.Length)
        {
            rotations = new float[GetSpawnData().numberOfBeans];
        }
        for (int i = 0; i < GetSpawnData().numberOfBeans; i++)
        {
            var fraction = (float)i / ((float)GetSpawnData().numberOfBeans - 1);
            var difference = GetSpawnData().maxRotation - GetSpawnData().minRotation;
            var fractionOfDifference = fraction * difference;
            rotations[i] = GetAngleOffset() + fractionOfDifference + GetSpawnData().minRotation; // We add minRotation to undo Difference
        }
        foreach (var r in rotations) ; //print(r);
    }

    public float GetAngleOffset()
    {
        Vector3 dir = (playerTarget.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Debug.Log("Angle2Player: " + angle.ToString());
        return angle;
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

        //Quaternion q = Quaternion.AngleAxis(GetAngleOffset(), Vector3.forward);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * rotationSpeed);

        // Spawn Beans
        GameObject[] spawnedBeans = new GameObject[GetSpawnData().numberOfBeans];
        for (int i = 0; i < GetSpawnData().numberOfBeans; i++)
        {
            spawnedBeans[i] = BeanPoolManager.GetBeanFromPool();
            if (spawnedBeans[i] == null)
            {
                spawnedBeans[i] = Instantiate(GetSpawnData().beanResource, transform.position, Quaternion.Euler(0, 0, rotations[i]), transform);
                BeanPoolManager.beans.Add(spawnedBeans[i]);
            } else
            {
                spawnedBeans[i].transform.SetParent(transform);
                spawnedBeans[i].transform.localPosition = Vector2.zero;
            }

            var b = spawnedBeans[i].GetComponent<Bean>();
            b.SetRotation(rotations[i]);
            b.speed = GetSpawnData().beanSpeed;
            b.velocity = GetSpawnData().beanVelocity;
            if (GetSpawnData().isParent == true)
            {
                spawnedBeans[i].transform.SetParent(null);
            }
        }
        return spawnedBeans;
    }
    
}