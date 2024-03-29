using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Fox_BaseState : FSMBaseState<FSM>
{
    static public readonly int idleState = Animator.StringToHash("Idle");

    static public readonly int wanderState = Animator.StringToHash("Wander");
    static public readonly int attackState = Animator.StringToHash("Attack");

    public SteeringAgent agent;
    protected Transform transform;
    protected Animator foxAnimator;

    protected Character warrior;

    public float sightDistance = 10.0f;
    public float fov = 70.0f;
    private float distance;

    protected bool canSee = false;

    public float ChangeStateTime;
    protected float stateTimer;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);

        agent=owner.GetComponent<SteeringAgent>();
        Debug.Assert(agent != null);
        foxAnimator = owner.GetComponentInChildren<Animator>();

        transform=owner.transform;
        ChangeStateTime = 5f;
        warrior = FindFirstObjectByType<Character>();
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
