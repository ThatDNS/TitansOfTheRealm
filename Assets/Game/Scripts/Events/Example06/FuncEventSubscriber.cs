using UnityEngine;

public class FuncEventSubscriber : MonoBehaviour
{
    // Reference to the FuncEventPublisher component on another GameObject
    public FuncEventPublisher funcEventPublisher;

    private void OnEnable()
    {
        // Safe check before subscribing to the Func event
        if (funcEventPublisher != null)
        {
            funcEventPublisher.OnFuncEvent += HandleFuncEvent;
        }
    }

    private void OnDisable()
    {
        // Safe check before unsubscribing to the Func event to prevent memory leaks
        if (funcEventPublisher != null)
        {
            funcEventPublisher.OnFuncEvent -= HandleFuncEvent;
        }
    }

    // The method to handle the Func event and return a string
    private string HandleFuncEvent(string message)
    {
        // Process the message and return a response
        return $"Received message: {message}";
    }
}

