using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_WanderState : Fox_BaseState
{
    private readonly int SpeedHashCode = Animator.StringToHash("Speed");
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.maxSpeed = 5;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        foxAnimator.SetFloat(SpeedHashCode, Mathf.Abs(agent.velocity.x));

        if (!CheckWarriorInRange())
        {
            fsm.ChangeState(idleState);
            foxAnimator.SetFloat(SpeedHashCode,0.0f);
        }
    }
}
