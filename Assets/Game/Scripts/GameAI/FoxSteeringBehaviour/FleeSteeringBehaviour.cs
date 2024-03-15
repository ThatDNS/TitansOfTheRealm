using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeSteeringBehaviour : SteeringBehaviourBase
{
    private Vector3 desiredVelocity;

    public Transform enemyTarget;
    public float fleeDistance = 5.0f;
    private bool showGizmoArrows = true;
    public override Vector3 CalculateForce()
    {   
        if (enemyTarget != null)
        {
            target = enemyTarget.position;
        }
        else
        {
            CheckMouseInput();
        }

        float distance = (transform.position - target).magnitude;
        if(distance>fleeDistance)
        {
            showGizmoArrows=false;
            return Vector3.zero;
        }

        showGizmoArrows = true;
        desiredVelocity= (transform.position-target).normalized;
        desiredVelocity *= steeringAgent.maxSpeed;
        return (desiredVelocity-steeringAgent.velocity);
    }

    private void OnDrawGizmos()
    {
        if(!showGizmoArrows)
        {
            return;
        }

        if(steeringAgent!=null)
        {
            DebugExtension.DebugArrow(transform.position, desiredVelocity, Color.red);
            DebugExtension.DebugArrow(transform.position, steeringAgent.velocity, Color.blue);
        }

        DebugExtension.DrawCircle(target, Vector3.up, Color.green, fleeDistance);
    }

}
