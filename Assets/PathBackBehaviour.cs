using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathBackBehaviour : StateMachineBehaviour
{
    public EnemyBehaviour enemy;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<EnemyBehaviour>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy.seeker.IsDone())
        {
            enemy.seeker.StartPath(enemy.rb.position, enemy.spawnPos, OnPathComplete);
        }
        if(Vector3.Distance(enemy.transform.position, enemy.spawnPos) < 15)
        {
            animator.SetTrigger("idle");
            animator.ResetTrigger("pathBack");
        }
        if (LineOfSight() && InRange())
        {
            animator.SetTrigger("combat");
            animator.ResetTrigger("pathBack");
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

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            enemy.path = p;
            enemy.currentWaypoint = 0;
        }
    }
}
