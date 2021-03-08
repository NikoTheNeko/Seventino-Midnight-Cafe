using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBH : MonoBehaviour {
    public AIPath aiPath;
    public enum DamageEnum { Slice, Fire, Flavor }

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
    public int health = 100;
    private Vector3 spawnPos;
    public int idleTimer;
    public int walkTimer;
    public int idleIndex = 0;
    public int losTimer = 3;
    public Vector3 originVec;
    public Vector3 flipVec;
    public Vector3 destVec;
    private bool canAttack = true;
    public LayerMask playerLayer;
    public GameObject SeedShot;

    public SpriteRenderer[] sprites;
    public Color hurtColor;

    public int totalFireDamage, totalFlavorDamage, totalSliceDamage;

    private void Start()
    {
        idleTimer = 1;
        walkTimer = 3;
        spawnPos = transform.position;
        rb = transform.GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        //seeker.StartPath(rb.position, rb.position, OnPathComplete);
        ai = GetComponent<IAstarAI>();

        originVec = transform.localScale;
        flipVec = originVec;
        flipVec.x *= -1;
    }

    void UpdatePath() 
    {
        switch (enemyState)
        {
            case "idle":
                EnemyIdle();
                break;

            case "combat":
                EnemyCombat();
                break;

            case "pathBack":
                EnemyPathBack();
                break;

            case "enrage":
                EnemyEnrage();
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
        //rb.AddForce(force);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
        if(ai.desiredVelocity.x >= .01f)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            //transform.localScale = theScale;
            enemyGFX.localScale = new Vector3(-0.25f, 0.25f, 1);
        }
        else if (ai.desiredVelocity.x <= .01f)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            //transform.localScale = theScale;
            enemyGFX.localScale = new Vector3(0.25f, 0.25f, 1);
        }
    }

    private void checkDead()
    {
        if (health < 0)
        {
            Debug.Log("Holy fuck I'm DEAD! LmaOO!!!");
            Debug.Log("Total Fire Damage: " + totalFireDamage);
            Debug.Log("Total Flavor Damage: " + totalFlavorDamage);
            Debug.Log("Total Slice Damage: " + totalSliceDamage);
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
    }

    Vector3 PickRandomPoint () {
        Vector3 point = Random.insideUnitSphere * 4;
        point.z = transform.position.z;
        point += ai.position;
        return point;
    }

    private void EnemyIdle()
    {
        if (Vector3.Distance(transform.position, spawnPos) > 20 && idleTimer == 0)
        {
            enemyState = ("pathBack");
        }
        if (LineOfSight() && InRange())
        {
            enemyState = ("combat");
        }
        if (idleIndex == 1)
        {
            Waiter();
        }
        if (idleIndex == 2)
        {
            Walker();
        }
        else
        {
            idleIndex = (int) Random.Range(1, 3);
        }
    }

    private void Waiter()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, rb.position, OnPathComplete);
        }
        if (idleTimer > 0)
        {
            idleTimer--;
        }
        else
        {
            idleTimer = (int)Random.Range(0, 3);
            idleIndex = 0;
        }
    }

    private void Walker()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, PickRandomPoint(), OnPathComplete);
        }
        if (walkTimer > 0)
        {
            walkTimer--;
        }
        else
        {
            walkTimer = (int)Random.Range(0, 2);
            idleIndex = 0;
        }
    }

    private bool InRange()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private bool LineOfSight()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(target.position, transform.position);
        Debug.DrawLine(transform.position, target.position, Color.white, 2.5f);
        if (raycastHit2D.collider != null)
        {
            if (raycastHit2D.transform.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void EnemyCombat()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

        if (Vector3.Distance(transform.position, target.transform.position) > 10)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
        }
        if(LineOfSight())
        {
            losTimer = 3;
        }
        if(!LineOfSight())
        {
            losTimer--;
        }

        int atkIndex = Ranges();
        switch (atkIndex)
        {
            case 1:
                Bite();
                break;
            case 2:
                ShotgunBeans();
                break;
            case 3:
                MachineBeans();
                break;
        }


        if(losTimer == 0)
        {
            enemyState = "idle";
        }
    }

    private int Ranges()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 3)
        {
            return 1;
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < 15)
        {
            return 2;
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < 30)
        {
            return 3;
        }
        return 0;
    }

    private void Bite()
    {
        if(canAttack)
        {
            canAttack = false;
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 2.5f, playerLayer);
            Debug.Log(hit.Length);
            foreach (Collider2D player in hit)
            {
                Debug.Log("collided");
                player.GetComponent<PlayerCombatTesting>().PlayerHit(10);
            }
            StartCoroutine(ResetAttack(1f));
        }
    }

    IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    private void ShotgunBeans()
    {
        if (canAttack)
        {
            canAttack = false;
            Instantiate(SeedShot, transform.position, Quaternion.identity);
            StartCoroutine(ResetAttack(1f));
        }
    }

    private void MachineBeans()
    {
        if (canAttack)
        {
            canAttack = false;
            Instantiate(SeedShot, transform.position, Quaternion.identity);
            StartCoroutine(ResetAttack(1f));
        }
    }

    private void EnemyEnrage()
    {
    }

    private void EnemyPathBack()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, spawnPos, OnPathComplete);
        }
        if(Vector3.Distance(transform.position, spawnPos) < 15)
        {
            enemyState = "idle";
        }
        if (LineOfSight() && InRange())
        {
            enemyState = "combat";
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
