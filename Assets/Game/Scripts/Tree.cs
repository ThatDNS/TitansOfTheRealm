using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : NetworkBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void UnFreezeRotation()
    {
        rb.freezeRotation = false;
    }
}
