using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReLoadAction : Action
{
    public SharedFloat reloadTime;

    public override TaskStatus OnUpdate()
    {
        reloadTime.Value-=Time.deltaTime;
        return TaskStatus.Success;
    }

}
