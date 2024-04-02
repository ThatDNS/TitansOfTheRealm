using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_MoveState : Spawner_BaseState
{
    public float MoveRadius = 10.0f;
    public float MoveSpeed = 2.0f;
    public Vector3 Target;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Target = new Vector3(Random.Range(-MoveRadius, MoveRadius), transform.position.y, Random.Range(-MoveRadius, MoveRadius));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Vector3.Distance(transform.position, Target) < 0.1f)
        {
            fsm.ChangeState(spawnState);
        }

        Vector3 currentPosition = transform.position;
        Vector3 direction = (Target - currentPosition).normalized;
        transform.position = currentPosition + direction * MoveSpeed * Time.deltaTime;
    }
}
