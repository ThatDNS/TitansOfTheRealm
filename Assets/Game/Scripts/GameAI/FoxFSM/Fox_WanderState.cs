using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_WanderState : Fox_BaseState
{
    private readonly int SpeedHashCode = Animator.StringToHash("Speed");
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.maxSpeed = 3;
        stateTimer = ChangeStateTime;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        foxAnimator.SetFloat(SpeedHashCode, Mathf.Abs(agent.velocity.x));
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            if (!CheckWarriorInRange())
            {
                fsm.ChangeState(idleState);
                foxAnimator.SetFloat(SpeedHashCode, 0.0f);
            }

            if (warrior != null)
            {
                canSee = false;
                Vector3 dirToTarget = warrior.transform.position - transform.position;
                Vector3 floor = transform.position;

                dirToTarget.y = 0.5f;
                floor.y = 0.5f;

                float viewAngle = Vector3.Angle(transform.forward, dirToTarget);
                if (viewAngle < fov * 0.5f)
                {
                    float dstToTarget = Vector3.Distance(transform.position, warrior.transform.position);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(transform.position, dirToTarget, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform == warrior.transform)
                        {
                            canSee = true;
                            fsm.ChangeState(attackState);
                        }
                    }
                }
            }
        }
    }
}
