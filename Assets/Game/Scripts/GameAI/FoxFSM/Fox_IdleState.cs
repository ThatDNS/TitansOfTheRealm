using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_IdleState : Fox_BaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.maxSpeed = 0;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (CheckWarriorInRange())
        {
            fsm.ChangeState(wanderState);
        }
    }

}
