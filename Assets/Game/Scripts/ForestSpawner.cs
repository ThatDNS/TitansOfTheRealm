using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSpawner : NetworkBehaviour
{
    [SerializeField] GameObject treeGO;
    [SerializeField] int numTrees = 5;

    private bool treesSpawned = false;

    private void Start()
    {
        treesSpawned = false;
    }

    public void SpawnTrees(NetworkRunner runner)
    {
        if (HasStateAuthority && !treesSpawned)
        {
            treesSpawned = true;
            int rows = (int) Mathf.Ceil(Mathf.Sqrt(numTrees));
            int cols = rows;
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    runner.Spawn(treeGO, new Vector3(-5.0f + i * 1.75f, 0, -5.0f + j * 1.75f));
                }
            }
        }
    }
}
