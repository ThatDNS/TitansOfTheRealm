using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskName("RunnerInstantiate")]
[TaskCategory("Generic")]
[TaskDescription("Instantiate object via runner")]
public class RunnerInstantiateAction : Action
{
    public SharedGameObject trapGameObject;
    NetworkRunner runner;

    public override void OnStart()
    {
        if (runner == null)
        {
            runner = GameObject.FindObjectOfType<NetworkRunner>();
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (runner.IsServer)
        {
            runner.Spawn(trapGameObject.Value, transform.position);
        }
        return TaskStatus.Success;
    }
}
