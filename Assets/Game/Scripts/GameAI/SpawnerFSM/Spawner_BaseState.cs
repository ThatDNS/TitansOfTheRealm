using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner_BaseState : FSMBaseState<FSM>
{
    static public readonly int moveState = Animator.StringToHash("Move");
    static public readonly int spawnState = Animator.StringToHash("Spawn");

    protected Transform transform;
    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        transform = owner.transform;
    }
}
