using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum DamageEnum { Slice, Fire, Flavor }
public class EnemyBehaviour : MonoBehaviour {
    public AIPath aiPath;

    //public Collider2D enemyHitbox;
    public Collider2D knifeHit;

    //mf to be killed
    public Transform target;
    public Transform enemyGFX;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public float attackRange;
    public IAstarAI ai;

    public Path path;
    public int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public Seeker seeker;
    public Rigidbody2D rb;

    public string enemyState = "idle";
    public int health = 100;
    public Vector3 spawnPos;

    public int idleIndex = 0;
    public int idleTimer;
    public int walkTimer;
    public int losTimer;
    public int biteTimer;
    public int shotgunTimer;
    public int machineTimer;

    public SpriteRenderer[] sprites;
    public Color hurtColor;

    public int totalFireDamage, totalFlavorDamage, totalSliceDamage;

    private InventoryTracker tracker;

    private void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("InventoryTracker");
        tracker = temp.GetComponent<InventoryTracker>();
        idleTimer = 300;
        walkTimer = 800;
        spawnPos = transform.position;
        rb = transform.GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        seeker.StartPath(rb.position, rb.position, OnPathComplete);
        ai = GetComponent<IAstarAI>();
    }


    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        rb.AddForce(force);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
        if(rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1, 1, 1);
        }
        else if (rb.velocity.x <= 0.01f)
        {
            enemyGFX.localScale = new Vector3(1, 1, 1);
        }
    }

    private void checkDead()
    {
        if (health < 0)
        {
            tracker.spawnFood("Brown Beans", totalSliceDamage, totalFireDamage, totalFlavorDamage, gameObject.transform.position);
            Debug.Log("Holy fuck I'm DEAD! LmaOO!!!");
            Debug.Log("Total Fire Damage: " + totalFireDamage);
            Debug.Log("Total Flavor Damage: " + totalFlavorDamage);
            Debug.Log("Total Slice Damage: " + totalSliceDamage);
            Destroy(gameObject);
        }
    }
    //maybe add aggro timer?
    //generally needs unfucking


    public bool LoS()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, target.position);
        if (raycastHit2D.collider != null)
        {
            PlayerCombatTesting target = raycastHit2D.collider.GetComponent<PlayerCombatTesting>();
            if (target != null)
            {
                return true;
            }
        }
        return false;
    }
    private void EnemyEscape()
    {
    }
    private void EnemyPathBack()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, spawnPos, OnPathComplete);
        }
    }

    public void TakeDamage(int amount, DamageEnum damageType){

        StartCoroutine(FlashColor());

        health -= amount;
        // Debug.Log("DAMAGED I REPEAT DAMAGED");

        switch (damageType)
        {
            case DamageEnum.Fire:
                totalFireDamage += amount;
                break;
            case DamageEnum.Flavor:
                totalFlavorDamage += amount;
                break;
            case DamageEnum.Slice:
                totalSliceDamage += amount;
                break;
        }

        checkDead();
    }

    IEnumerator FlashColor()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = hurtColor;
        }
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = Color.white;
        }
    }
}
