using UnityEngine;

public class ActionEventSubscriber : MonoBehaviour
{
    // Reference to the ActionEventPublisher component on another GameObject
    public ActionEventPublisher actionEventPublisher;

    private void OnEnable()
    {
        // Safe check before subscribing to the event
        if (actionEventPublisher != null)
        {
            actionEventPublisher.OnActionEvent += HandleActionEvent;
        }
    }

    private void OnDisable()
    {
        // Safe check before unsubscribing to the event to prevent memory leaks
        if (actionEventPublisher != null)
        {
            actionEventPublisher.OnActionEvent -= HandleActionEvent;
        }
    }

    // The method to handle the Action event
    private void HandleActionEvent(string message)
    {
        // Handle the event, for example, by logging the message
        Debug.Log($"Action event received with message: {message}");
    }
}
