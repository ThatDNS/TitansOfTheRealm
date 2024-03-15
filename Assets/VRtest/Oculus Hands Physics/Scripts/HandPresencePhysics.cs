using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPresencePhysics : NetworkBehaviour
{
    public enum TitanHand
    {
        Left,
        Right
    }

    public Transform titan = null;
    public Transform titanHand = null;
    public TitanHand titanHandType = TitanHand.Left;

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

    // HandPresencePhysics.titan must be set before finding its hand
    public void FindTitanHand()
    {
        if (titan == null)
            return;

        string titanHandTag = (titanHandType == TitanHand.Left) ? "TitanLeftHand" : "TitanRightHand";
        GameObject _titanHand = FindObjectWithTag(titan, titanHandTag);
        if (_titanHand != null)
        {
            Debug.Log("Found " + titanHandType + " titan hand");
            titanHand = _titanHand.transform;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (titanHand == null)
            return;

        rb.velocity = (titanHand.position - transform.position) / Time.fixedDeltaTime;
        Quaternion rotDiff = titanHand.rotation * Quaternion.Inverse(transform.rotation);
        rotDiff.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDiffInDegree = angleInDegree * rotationAxis;
        rb.angularVelocity = (rotationDiffInDegree * Mathf.Deg2Rad) / Time.fixedDeltaTime;
    }

    // Recursive method to search for GameObject with the specified tag
    GameObject FindObjectWithTag(Transform parent, string tag)
    {
        Debug.Log("Looking for tag '" + tag + "' in " + parent.gameObject.name);
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }

            GameObject foundObject = FindObjectWithTag(child, tag);
            if (foundObject != null)
            {
                return foundObject;
            }
        }

        return null;
    }
}
