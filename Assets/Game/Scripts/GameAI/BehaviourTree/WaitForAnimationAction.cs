using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskName("Wait for Animation")]
[TaskCategory("Generic")]
[TaskDescription("wait for an animation to be completed via the Animation Listener")]
public class WaitForAnimationAction : Action
{
    public SharedString animationName1;
    public SharedString animationName2;
    public SharedString animationName3;

    private AnimationListener animationListener;

    private bool animationCompleted = false;
    private List<int> animationHashCodes = new List<int>();



    public override void OnStart()
    {
        animationCompleted = false;
        animationListener = gameObject.GetComponent<AnimationListener>();
        if (animationName1 != null )
        {
            AddAnimationListener(animationName1.Value);
        }
        if(animationName2 != null)
        {
            AddAnimationListener(animationName2.Value);
        }
        if(animationName3 != null)
        {
            AddAnimationListener(animationName3.Value);
        }
    }

    private void OnAnimationCompleted(int hashCode)
    {
        if (animationHashCodes.Contains(hashCode))
        {
            animationCompleted = true;
        }
    }
    private void AddAnimationListener(string animationName)
    {
        int hash = Animator.StringToHash(animationName);
        animationHashCodes.Add(hash);
        animationListener.AddAnimationCompletedListener(hash, OnAnimationCompleted);
    }
    public override TaskStatus OnUpdate()
    {
        if (animationCompleted == true)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        foreach (var hash in animationHashCodes)
        {
            animationListener.RemoveAnimationCompletedListener(hash, OnAnimationCompleted);
        }
    }
}
