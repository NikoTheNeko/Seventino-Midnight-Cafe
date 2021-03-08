using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IdleBehaviour : StateMachineBehaviour
{
    public EnemyBehaviour enemy;
   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
        enemy = animator.GetComponent<EnemyBehaviour>();
        //InvokeRepeating("Walk", 0f, 0.5f);
   }

    void Walk()
    {
        if (enemy.idleIndex == 1)
        {
            Waiter();
        }
        if (enemy.idleIndex == 2)
        {
            Walker();
        }
        else
        {
            enemy.idleIndex = Random.Range(1, 2);
        }
    }

   // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
        if (Vector3.Distance(enemy.transform.position, enemy.spawnPos) > 20 && enemy.idleTimer == 0)
        {
            animator.SetTrigger("pathBack");
            animator.ResetTrigger("idle");
        }
        if(LineOfSight() && InRange())
        {
            animator.SetTrigger("combat");
            animator.ResetTrigger("idle");
        }
    }

    private bool InRange()
    {
        if (Vector3.Distance(enemy.transform.position, enemy.target.transform.position) < enemy.attackRange)
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
        RaycastHit2D raycastHit2D = Physics2D.Raycast(enemy.transform.position, enemy.target.position);
        if (raycastHit2D.collider != null)
        {
            if (raycastHit2D.transform == enemy.target)
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

    private void Waiter()
    {
        if (enemy.seeker.IsDone())
        {
            enemy.seeker.StartPath(enemy.rb.position, enemy.rb.position, OnPathComplete);
        }
        if (enemy.idleTimer > 0)
        {
            enemy.idleTimer--;
        }
        else
        {
            enemy.idleTimer = (int)Random.Range(0, 400);
            enemy.idleIndex = 0;
        }
    }

    private void Walker()
    {
        if (enemy.seeker.IsDone())
        {
            enemy.seeker.StartPath(enemy.rb.position, PickRandomPoint(), OnPathComplete);
        }
        if (enemy.walkTimer > 0)
        {
            enemy.walkTimer--;
        }
        else
        {
            enemy.walkTimer = (int)Random.Range(0, 600);
            enemy.idleIndex = 0;
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            enemy.path = p;
            enemy.currentWaypoint = 0;
        }
    }

    Vector3 PickRandomPoint()
    {
        Vector3 point = Random.insideUnitSphere * 4;
        point += enemy.ai.position;
        point.z = enemy.transform.position.z;
        return point;
    }
}
