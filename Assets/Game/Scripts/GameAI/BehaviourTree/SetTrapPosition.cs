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
 
        GetRandomPlacementPosition();
       
    }

    private void GetRandomPlacementPosition()
    {
        Vector3 randomPoint = Random.insideUnitSphere * placementRadius.Value;
        randomPoint.y = 0; // Assuming you're placing the trap on a flat surface.
        trapPosition.Value = transform.position + randomPoint;

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
