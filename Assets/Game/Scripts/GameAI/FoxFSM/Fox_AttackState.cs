using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_AttackState : Fox_BaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        agent.maxSpeed = 10;
        stateTimer = ChangeStateTime;
        animator.SetFloat("Speed", 1.0f);
        attackability.enabled = true;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        stateTimer-=Time.deltaTime;

        if(stateTimer < 0)
        {
            fsm.ChangeState(wanderState);
        }
    }
}
