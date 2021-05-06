using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFloatingTest : MonoBehaviour
{
    [SerializeField] private float destroyInSecs = 1f;
    public Vector3 offset = new Vector3(0, 30f, 0);
    public Vector3 randomizeSpawn = new Vector3(3f, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyInSecs);

        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-randomizeSpawn.x, randomizeSpawn.y),
            Random.Range(-randomizeSpawn.x, randomizeSpawn.y),
            Random.Range(-randomizeSpawn.x, randomizeSpawn.y));
    }
}
