using UnityEngine;
using System;

public class ActionEventPublisher : MonoBehaviour
{
    // Define an Action event
    public event Action<string> OnActionEvent;

    // Method to trigger the event
    public void RaiseActionEvent()
    {
        OnActionEvent?.Invoke("Hello from ActionEventPublisher!");
    }

    // This method could be linked to a UI button or other game action
    public void TriggerEvent()
    {
        RaiseActionEvent();
    }
}
