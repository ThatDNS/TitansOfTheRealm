using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_BaseState : FSMBaseState<FSM>
{
    static public readonly int IdleState = Animator.StringToHash("Idle");

    static public readonly int WanderState = Animator.StringToHash("Wander");

    protected SteeringAgent agent;
    protected Transform transform;
    protected Animator foxAnimator;


    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);

        agent=owner.GetComponent<SteeringAgent>();

        foxAnimator=owner.GetComponent<Animator>();

        transform=owner.transform;
    }


}
