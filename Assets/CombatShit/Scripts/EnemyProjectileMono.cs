using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileMono : MonoBehaviour
{
    public float projSpeed;

    public Transform target;
    private Vector2 path;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;

        path = new Vector2(target.position.x, target.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, path, projSpeed * Time.deltaTime);

        if (transform.position.x == path.x && transform.position.y == path.y)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCombatTesting>().PlayerHit(5);
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
