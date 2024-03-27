using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTitan : NetworkBehaviour
{
    public bool IsLocalNetworkRig => Object.HasInputAuthority;

    public override void FixedUpdateNetwork()
    {
        if (IsLocalNetworkRig)
        {
            transform.position += transform.forward * Time.fixedDeltaTime;
        }
    }
}
