using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//public enum DamageEnum { Slice, Fire, Flavor }
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
    IAstarAI ai;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public Seeker seeker;
    public Rigidbody2D rb;

    public string enemyState = "idle";
    private int health = 1;
    private Vector3 spawnPos;
    public int idleTimer;
    public int walkTimer;

    private void Start()
    {
        idleTimer = 1;
        walkTimer = 3;
        spawnPos = transform.position;
        //seeker = transform.GetChild(0).GetComponent<Seeker>();
        //rb = transform.GetChild(0).GetComponent<Rigidbody2D>();
        rb = transform.GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        seeker.StartPath(rb.position, rb.position, OnPathComplete);
        ai = GetComponent<IAstarAI>();
    }

    void UpdatePath() 
    {
        switch (enemyState)
        {
            case "idle":
                EnemyIdle();
                break;

            case "walkAround":
                EnemyWalk();
                break;

            case "combat":
                EnemyCombat();
                break;

            case "escape":
                EnemyEscape();
                break;

            case "goHome":
                EnemyGoHome();
                break;
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update(){
        checkDead();
        checkCombat();
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
            Debug.Log("Holy fuck I'm DEAD! LmaOO!!!");
            Destroy(gameObject);
        }
    }
    //maybe add aggro timer?
    //generally needs unfucking
    private void checkCombat()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
        {
            enemyState = "combat";
        }
        else
        {
            enemyState = "idle";
        }
    }

    Vector3 PickRandomPoint () {
        Vector3 point = Random.insideUnitSphere * 4;
        point.z = transform.position.z;
        point += ai.position;
        return point;
    }

    private void EnemyIdle()
    {
        if(seeker.IsDone()) 
        {
            seeker.StartPath(rb.position, rb.position, OnPathComplete);
        }
        /*if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
        {
            ai.destination = ai.position;
            ai.SearchPath();
        }*/
        if (idleTimer > 0)
        {
            idleTimer--;
        }
        else
        {
            idleTimer = (int)Random.Range(1, 4);
            enemyState = "walkAround";
        }
    }

    private void EnemyWalk()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, PickRandomPoint(), OnPathComplete);
        }
        /*if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath)) {
            ai.destination = PickRandomPoint();
            ai.SearchPath();
        }*/
        if (walkTimer > 0)
        {
            walkTimer--;
            //move
        }
        else
        {
            walkTimer = (int)Random.Range(1, 2);
            enemyState = "idle";
        }
    }
    private void EnemyCombat()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
    private void EnemyEscape()
    {
    }
    private void EnemyGoHome()
    {
    }

    public void TakeDamage(int amount){
        health -= amount;
    }
}
