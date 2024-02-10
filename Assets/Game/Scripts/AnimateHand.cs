using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHand : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    private Animator animator = null;
    
    [SerializeField] private float smoothness = 5.0f;
    [SerializeField] private string gripStateName = "Grip";
    [SerializeField] private string triggerStateName = "Trigger";
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
        float gripValue = gripAnimationAction.action.ReadValue<float>();

        float smoothTriggerValue = Mathf.Lerp(animator.GetFloat(triggerState), triggerValue, Time.deltaTime * smoothness);
        float smoothGripValue = Mathf.Lerp(animator.GetFloat(gripState), gripValue, Time.deltaTime * smoothness);

        animator.SetFloat(triggerState, smoothTriggerValue);
        animator.SetFloat(gripState, smoothGripValue);
    }
}
