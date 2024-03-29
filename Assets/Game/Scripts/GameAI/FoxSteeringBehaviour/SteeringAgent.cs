using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    public enum SummingMethod
    {
        WeightedAverage,
        Prioritized,
    };

    public SummingMethod summingMethod = SummingMethod.WeightedAverage;

    public bool useRootMotion = true;
    public bool useGravity = true;

    private Animator animator;
    private CharacterController characterController;


    public float mass = 1.0f;
    public float maxSpeed = 1.0f;
    public float maxForce = 10.0f;
    public bool reachedGoal = false;

    public Vector3 velocity = Vector3.zero;

    private List<SteeringBehaviourBase> steeringBehaviours = new List<SteeringBehaviourBase>();

    public float angularDampeningTime = 5.0f;
    public float deadZone = 10.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            useRootMotion = false;
        }
        characterController = GetComponent<CharacterController>();
        steeringBehaviours.AddRange(GetComponentsInChildren<SteeringBehaviourBase>());
        foreach (SteeringBehaviourBase behaviour in steeringBehaviours)
        {
            behaviour.steeringAgent = this;
        }
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 steeringForce = CalculateSteeringForce();

        if (reachedGoal == true)
        {
            velocity = Vector3.zero;
            if (animator != null)
                animator.SetFloat("Speed", 0);
        }
        else
        {
            Vector3 accerleration = steeringForce / mass;

            velocity = velocity + (accerleration * Time.deltaTime);

            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            float speed = velocity.magnitude;
            if (animator != null)
            {
                animator.SetFloat("Speed", speed);
            }

            if (useRootMotion == false)
            {
                if (characterController != null)
                {
                    characterController.Move(velocity * Time.deltaTime);
                }
                else
                {
                    transform.position += (velocity * Time.deltaTime);
                }

                if (useGravity == true)
                {
                    characterController.Move(Physics.gravity * Time.deltaTime);
                }
            }

            if (velocity.magnitude > 0.0f)
            {
                velocity.y = 0;
                float angle = Vector3.Angle(transform.forward, velocity);

                if (Mathf.Abs(angle) <= deadZone)
                {
                    transform.LookAt(transform.position + velocity);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                                                        Quaternion.LookRotation(velocity),
                                                        Time.deltaTime * angularDampeningTime);
                }
            }
        }
    }

    private Vector3 CalculateSteeringForce()
    {
        Vector3 totalForce = Vector3.zero;
        foreach (SteeringBehaviourBase behaviour in steeringBehaviours)
        {
            if (behaviour.enabled)
            {
                switch (summingMethod)
                {
                    case SummingMethod.WeightedAverage:
                        totalForce = totalForce + (behaviour.CalculateForce() * behaviour.weight);
                        totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
                        break;
                    case SummingMethod.Prioritized:
                        Vector3 steeringForce = (behaviour.CalculateForce() * behaviour.weight);
                        if (!AccumulateForce(ref totalForce, steeringForce))
                        {
                            return totalForce;
                        }

                        break;
                }
            }
        }

        return totalForce;
    }

    bool AccumulateForce(ref Vector3 RunningTot, Vector3 ForceToAdd)
    {
        float MagnitudeSoFar = RunningTot.magnitude;

        float MagnitudeRemaining = maxForce - MagnitudeSoFar;

        if (MagnitudeRemaining <= 0)
        {
            return false;
        }

        float MagnitudeToAdd = ForceToAdd.magnitude;

        if (MagnitudeToAdd < MagnitudeRemaining)
        {
            RunningTot = RunningTot + ForceToAdd;
        }
        else
        {
            RunningTot = RunningTot + (ForceToAdd.normalized * MagnitudeRemaining);
        }

        return true;
    }

    private void OnAnimatorMove()
    {
        if (Time.deltaTime != 0.0f && useRootMotion == true)
        {
            Vector3 animatonVelocity = animator.deltaPosition / Time.deltaTime;
            if (characterController != null)
            {
                characterController.Move((transform.forward * animatonVelocity.magnitude) * Time.deltaTime);
            }
            else
            {
                transform.position += (transform.forward * animatonVelocity.magnitude) * Time.deltaTime;
            }

            if (useGravity == true)
            {
                characterController.Move(Physics.gravity * Time.deltaTime);
            }
        }
    }
}
