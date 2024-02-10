using UnityEngine;

public class EventSubscriberCustom : MonoBehaviour
{
    // Reference to the EventPublisher component on another GameObject
    public EventPublisherCustom eventPublisher;

    private void OnEnable()
    {
        // Safe check before subscribing to the event
        if (eventPublisher != null)
        {
            eventPublisher.OnCustomEvent += HandleCustomEvent;
        }
    }

    private void OnDisable()
    {
        // Safe check before unsubscribing to the event to prevent memory leaks
        if (eventPublisher != null)
        {
            eventPublisher.OnCustomEvent -= HandleCustomEvent;
        }
    }

    // The event handler method
    private void HandleCustomEvent(object sender, CustomEventArgs e)
    {
        // Handle the event, for example by logging the message to the Unity console
        Debug.Log($"Custom event received with message: {e.Message}");
    }
}
