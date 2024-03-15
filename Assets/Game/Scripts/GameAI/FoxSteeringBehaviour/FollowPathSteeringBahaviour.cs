using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPathSteeringBahaviour : ArriveSteeringBehaviour
{
    public float waypointDistance = 0.5f;
    public int currentWaypointIndex = 0;
    private NavMeshPath path;

    private void Start()
    {
        path=new NavMeshPath();
    }
    public override Vector3 CalculateForce()
    {
        CheckMouseInput();

        if(mouseClicked)
        {
            currentWaypointIndex = 0;
            NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (path.corners.Length > 0)
            {
                target = path.corners[0];
            }
            else
            {
                target=transform.position;
            }
        }

        if (currentWaypointIndex != path.corners.Length && (target - transform.position).magnitude < waypointDistance)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex < path.corners.Length)
            {
                target = path.corners[currentWaypointIndex];
            }
        }


        return CalculateArriveForce();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        DebugExtension.DrawCircle(target, Color.magenta, waypointDistance);
        if (path!=null)
        {
            for(int i =1;i<path.corners.Length;i++)
            {
                Debug.DrawLine(path.corners[i-1], path.corners[i], Color.black);
            }
        }
    }
}
