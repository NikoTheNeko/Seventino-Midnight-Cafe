using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CombatBehaviour : StateMachineBehaviour
{
    public EnemyBehaviour enemy;
    private bool canAttack = true;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<EnemyBehaviour>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.target.transform.position) > 10)
        {
            if (enemy.seeker.IsDone())
            {
                enemy.seeker.StartPath(enemy.rb.position, enemy.target.position, OnPathComplete);
            }
        }
        if(LineOfSight())
        {
            enemy.losTimer = 3;
        }
        if(!LineOfSight())
        {
            enemy.losTimer--;
        }

        int atkIndex = Ranges();
        switch (atkIndex)
        {
            case 0:
                Bite();
                break;
            case 1:
                ShotgunBeans();
                break;
            case 2:
                MachineBeans();
                break;
        }


        if(enemy.losTimer == 0)
        {
            animator.SetTrigger("idle");
            animator.ResetTrigger("combat");
        }
    }

    private void Bite()
    {
        //reset other atk timers
    }

    private void ShotgunBeans()
    {

    }

    private void MachineBeans()
    {

    }

    private int Ranges()
    {
        if (Vector3.Distance(enemy.transform.position, enemy.target.transform.position) < 30)
        {
        }
        return 0;
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
