using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
public class CanPlaceTrap : Conditional
{
    public SharedFloat timer;
    public override TaskStatus OnUpdate()
    {
        return timer.Value <0 ? TaskStatus.Success : TaskStatus.Failure;

    }
}
