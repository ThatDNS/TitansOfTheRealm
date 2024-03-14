using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderSteeringBahaviour : SeekSteeringBehaviour
{
    public float wanderDistance = 2.0f;
    public float wanderRadius = 1.0f;
    public float wanderJitter = 20.0f;
    private Vector3 wanderTarget;

    private void Start()
    {
        float theta =(float)Random.value*Mathf.PI*2.0f;
        wanderTarget = new Vector3(wanderRadius*Mathf.Cos(theta), 0.0f, wanderRadius*Mathf.Sin(theta));
    }

    public override Vector3 CalculateForce()
    {
        float wanderJitterTimeSlice=wanderJitter*Time.deltaTime;
        wanderTarget += new Vector3(
            Random.Range(-1.0f, 1.0f)*wanderJitterTimeSlice,
            0.0f,
            Random.Range(-wanderJitterTimeSlice, wanderJitterTimeSlice));
        wanderTarget.Normalize();
        wanderTarget *= wanderDistance;

        target = wanderTarget + new Vector3(0, 0, wanderDistance);

        target=steeringAgent.transform.rotation*target+steeringAgent.transform.position;

        return base.CalculateSeekForce();
    }

    protected override void OnDrawGizmos()
    {
        Vector3 circle=transform.rotation* new Vector3(0.0f, 0.0f,wanderDistance)+transform.position;
        DebugExtension.DrawCircle(circle, Vector3.up, Color.red,wanderRadius);
        Debug.DrawLine(transform.position, circle, Color.red);
        Debug.DrawLine(transform.position, target, Color.blue);
    }
}
