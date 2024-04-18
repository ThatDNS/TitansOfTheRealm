using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskName("Patrol")]
[TaskCategory("Generic")]
[TaskDescription("Patrols between waypoints")]

public class PatrolAction : MoveToGoalAction
{
    public SharedFloat reloadTime;
    public SharedGameObjectList waypoints;
    public bool loop = true;
    private int index = 0;
    private bool ranFirstTime = true;

    public override void OnStart()
    {
        base.OnStart();
        if (ranFirstTime)
        {
            ranFirstTime = false;
            waypoints.Value.Clear();
            GameObject[] waypointsGO = GameObject.FindGameObjectsWithTag("TrapPlacerWaypoint");
            foreach (GameObject obj in waypointsGO)
            {
                waypoints.Value.Add(obj);
            }
            Debug.Log("Trap placer found " + waypoints.Value.Count + " waypoints!");
        }
        if (index < waypoints.Value.Count)
        {
            agent.isStopped = false;
            agent.SetDestination(waypoints.Value[index].transform.position);
        }
    }

    public override TaskStatus OnUpdate()
    {
        TaskStatus baseStatus = base.OnUpdate();
        reloadTime.Value -= Time.deltaTime;
        if (baseStatus != TaskStatus.Running && index != waypoints.Value.Count)
        {
            index++;
            if (index >= waypoints.Value.Count && loop == true)
            {
                index = 0;
            }

            if (index < waypoints.Value.Count)
            {
                agent.SetDestination(waypoints.Value[index].transform.position);
                return TaskStatus.Running;
            }
        }


        return baseStatus;
    }
}
