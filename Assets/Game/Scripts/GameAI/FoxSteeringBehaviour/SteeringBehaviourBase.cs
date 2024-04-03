using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public abstract class SteeringBehaviourBase : MonoBehaviour
{
    public float weight = 1.0f;
    public Vector3 target = Vector3.zero;
    public Character warrior;
    public abstract Vector3 CalculateForce();
    

    [HideInInspector] public SteeringAgent steeringAgent;

    public bool useMouseInput = true;
    protected bool mouseClicked= false;


    protected void CheckMouseInput()
    {
        mouseClicked = false;
        if (Input.GetKeyDown(KeyCode.Q) && useMouseInput)
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, 100))
            //{
            //    target = hit.point;
            //    mouseClicked = true;
            //}
            target = warrior.transform.position;
            target.y= 0;
            mouseClicked = true;

            foreach(SteeringBehaviourBase s in steeringAgent.steeringBehaviours)
            {
                if (s is WanderSteeringBahaviour)
                {
                    s.weight = 0f;
                }
                else if (s is SeekSteeringBehaviour)
                {
                    s.weight = 1.0f;
                }
                else if (s is FleeSteeringBehaviour)
                {
                    s.weight = 0f;
                }
            }
            
        }
    }


}
