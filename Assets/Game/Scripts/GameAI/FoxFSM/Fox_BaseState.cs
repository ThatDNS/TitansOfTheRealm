using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Fox_BaseState : FSMBaseState<FSM>
{

    static public readonly int wanderState = Animator.StringToHash("Wander");
    static public readonly int attackState = Animator.StringToHash("Attack");

    public SteeringAgent agent;
    protected Transform transform;
    protected Animator foxAnimator;
    protected DamageOnTouch attackability;
    protected Character warrior;

    private float distance;

    public float ChangeStateTime;
    protected float stateTimer;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);

        agent=owner.GetComponent<SteeringAgent>();
        Debug.Assert(agent != null);
        foxAnimator = owner.GetComponentInChildren<Animator>();
        attackability=owner.GetComponent<DamageOnTouch>();
        transform=owner.transform;
        ChangeStateTime = 5f;
        warrior = FindFirstObjectByType<Character>();
        Debug.Assert(warrior != null, $"{owner.name}'s warrior not found");
    }

    protected bool CheckWarriorInRange()
    {
        if (warrior == null) { return false; }

        distance=Vector3.Distance(transform.position, warrior.transform.position);

        
        if (distance > 2.0f) { return false; } 

        return true;

    }



}
