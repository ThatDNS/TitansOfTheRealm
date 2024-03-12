using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPresencePhysics : MonoBehaviour
{
    [Tooltip("VR Hand transform")]
    public Transform target;

    private Rigidbody rb;
    private Collider[] handColliders;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<Collider>();
    }

    public void EnableHandCollider()
    {
        foreach (var collider in handColliders)
        {
            collider.enabled = true;
        }
    }

    public void EnableHandColliderWithDelay(float delay)
    {
        Invoke("EnableHandCollider", delay);
    }

    public void DisableHandCollider()
    {
        foreach (var collider in handColliders)
        {
            collider.enabled = false;
        }
    }

    public void ModifyTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;
        Quaternion rotDiff = target.rotation * Quaternion.Inverse(transform.rotation);
        rotDiff.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDiffInDegree = angleInDegree * rotationAxis;
        rb.angularVelocity = (rotationDiffInDegree * Mathf.Deg2Rad) / Time.fixedDeltaTime;
    }
}
