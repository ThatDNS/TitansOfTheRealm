using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Spawner_SpawnState : Spawner_BaseState
{
    public GameObject spawnedObject;

    public float Timer;
    public float spawnTime=5.0f;

    NetworkRunner runner;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        runner = FindObjectOfType<NetworkRunner>();
        Timer = spawnTime;
        if (runner.IsServer)
            runner.Spawn(spawnedObject, transform.position, transform.rotation);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        Timer-=Time.deltaTime;
        if(Timer<=0)
        {
            fsm.ChangeState(moveState);
        }

        
    }
}
