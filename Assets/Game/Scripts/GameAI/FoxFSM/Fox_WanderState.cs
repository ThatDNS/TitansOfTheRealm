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
        attackability.enabled = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            if (CheckWarriorInRange())
            {
                foxAnimator.SetFloat(SpeedHashCode, 0f);
                fsm.ChangeState(attackState);
            }

        }
    }
}
