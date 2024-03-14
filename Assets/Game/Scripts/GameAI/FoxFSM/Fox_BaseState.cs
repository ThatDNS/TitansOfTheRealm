using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fox_BaseState : FSMBaseState<FSM>
{
    static public readonly int idleState = Animator.StringToHash("Idle");

    static public readonly int wanderState = Animator.StringToHash("Wander");

    protected SteeringAgent agent;
    protected Transform transform;
    protected Animator foxAnimator;

    protected Character warrior;


    public float distance;
    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);

        agent=owner.GetComponent<SteeringAgent>();

        foxAnimator = owner.GetComponentInChildren<Animator>();

        transform=owner.transform;

        warrior = GameObject.FindFirstObjectByType<Character>();
        Debug.Assert(warrior != null, $"{owner.name}'s warrior not found");
    }

    protected bool CheckWarriorInRange()
    {
        if (warrior == null) { return false; }

        distance=Vector3.Distance(transform.position, warrior.transform.position);

        
        if (distance > 10) { return false; } 

        return true;

    }


}
