using System;
using UnityEngine;

public class EventSubscriber : MonoBehaviour
{
    // Reference to the EventPublisher component on
    // another GameObject
    public EventPublisher eventPublisher;

    private void OnEnable()
    {
        // Ensure the eventPublisher reference is set correctly,
        // for example via the inspector
        if (eventPublisher != null)
        {
            // Subscribe to the event when the GameObject is enabled
            eventPublisher.OnSimpleEvent += HandleSimpleEvent;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to the event when the GameObject is disabled
        // to prevent memory leaks
        if (eventPublisher != null)
        {
            eventPublisher.OnSimpleEvent -= HandleSimpleEvent;
        }
    }

    // The event handler method
    private void HandleSimpleEvent(object sender, EventArgs e)
    {
        // Handle the event, for example by logging to the console
        Debug.Log("Simple event received via MonoBehaviour.");
    }
}
