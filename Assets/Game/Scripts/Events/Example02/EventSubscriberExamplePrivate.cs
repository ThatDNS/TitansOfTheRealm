using UnityEngine;

public class EventSubscriberExamplePrivate : MonoBehaviour
{
    private EventTriggerExamplePrivate eventTrigger;

    private void OnEnable()
    {
        eventTrigger ??= transform.GetComponent<EventTriggerExamplePrivate>();
        // Subscribe to the event when the GameObject is enabled
        eventTrigger?.Subscribe(HandleMessageEvent);
    }

    private void OnDisable()
    {
        // Unsubscribe to the event when the GameObject is disabled
        // This is important to prevent memory leaks
        eventTrigger?.Unsubscribe(HandleMessageEvent);
    }

    // The event handler method
    private void HandleMessageEvent(string message)
    {
        // Action to take when the event is triggered
        Debug.Log($"Message received: {message}");
    }
}

