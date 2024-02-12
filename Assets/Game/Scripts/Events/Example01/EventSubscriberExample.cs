using UnityEngine;

public class EventSubscriberExample : MonoBehaviour 
{
    private void OnEnable()
    {
        // Subscribe to the event when the GameObject is enabled
        EventTriggerExample.OnMessageReceived += HandleMessageEvent;
    }

    private void OnDisable()
    {
        // Unsubscribe to the event when the GameObject is disabled
        EventTriggerExample.OnMessageReceived -= HandleMessageEvent;
    }

    // The event handler method
    private void HandleMessageEvent(string message)
    {
        // Action to take when the event is triggered
        Debug.Log($"Message received: {message}");
    }
}

