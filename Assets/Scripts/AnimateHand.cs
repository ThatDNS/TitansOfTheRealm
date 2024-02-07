using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHand : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    
    private Animator animator = null;
    public string gripStateName = "Grip";
    public string triggerStateName = "Trigger";
    private int gripState;
    private int triggerState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gripState = Animator.StringToHash(gripStateName);
        triggerState = Animator.StringToHash(triggerStateName);
    }

    private void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        animator.SetFloat(triggerState, triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        animator.SetFloat(gripState, gripValue);
    }
}
