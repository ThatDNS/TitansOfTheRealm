using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Fox_IdleState : Fox_BaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.maxSpeed = 0;
        stateTimer = ChangeStateTime;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateTimer-=Time.deltaTime;
        if (CheckWarriorInRange()&&stateTimer<=0)
        {
            fsm.ChangeState(wanderState);
        }
    }

}
