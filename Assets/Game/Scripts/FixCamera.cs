using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCamera : MonoBehaviour
{
    private Transform follow = null;
    [SerializeField] Transform titanCameraOffset;
    [SerializeField] Transform warriorCameraOffset;

    private void Update()
    {
        if (follow != null)
        {
            transform.position = follow.position;
            transform.rotation = follow.rotation;
        }
    }

    public void AttachToTitan()
    {
        follow = titanCameraOffset;
    }

    public void AttachToWarrior()
    {
        follow = warriorCameraOffset;
    }
}
