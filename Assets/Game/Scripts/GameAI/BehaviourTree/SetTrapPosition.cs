using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskName("SetTrapAction")]
[TaskCategory("Generic")]
[TaskDescription("Placing Trap")]
public class SetTrapPosition : Action
{

    public SharedFloat placementRadius = 5.0f;
    public SharedVector3 trapPosition;

    public override void OnStart()
    {
 
        trapPosition = GetRandomPlacementPosition();
       
    }

    private Vector3 GetRandomPlacementPosition()
    {
        Vector3 randomPoint = Random.insideUnitSphere * placementRadius.Value;
        randomPoint.y = 0; // Assuming you're placing the trap on a flat surface.
        Vector3 placementPosition = transform.position + randomPoint;

        Debug.Log("Placement Position: " + placementPosition);

        return placementPosition;
    }

    public override TaskStatus OnUpdate()
    {

        return TaskStatus.Success;
    }
    public override void OnReset()
    {

        trapPosition = Vector3.zero;
    }
}
