using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSpawner : NetworkBehaviour
{
    [SerializeField] GameObject treeGO;

    public void SpawnTree()
    {
        if (HasStateAuthority)
        {
            Runner.Spawn(treeGO, new Vector3(3.462f, -3.364f, -2.834f));
        }
    }
}
